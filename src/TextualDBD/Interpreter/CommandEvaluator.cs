using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB;
using TextualDB.Components;
using TextualDB.Exceptions;

using TextualDBD.Interpreter.Ast;

namespace TextualDBD.Interpreter
{
    public class CommandEvaluator : IVisitor
    {
        public TextualDBReader Reader { get; private set; }
        public TextualDBWriter Writer { get; private set; }
        public TextualDBDatabase Database { get; private set; }
        public string DatabaseFile { get; private set; }

        public CommandEvaluator(string databaseFile)
        {
            Reader = new TextualDBReader(databaseFile);
            Writer = new TextualDBWriter();
            Database = Reader.Read();
            DatabaseFile = databaseFile;
        }

        private StringBuilder response;
        private Stack<string> stack;
        private Stack<TextualDBTable> tableStack;
        private Stack<TextualDBRow> rowStack;

        public string Execute(AstNode command)
        {
            stack = new Stack<string>();
            tableStack = new Stack<TextualDBTable>();
            rowStack = new Stack<TextualDBRow>();
            response = new StringBuilder();

            command.Visit(this);
            return response.ToString();
        }

        public void WriteChanges(string databaseFile = "")
        {
            Writer.Write(Database, databaseFile == string.Empty ? DatabaseFile : databaseFile);
        }

        public void Accept(BinaryExpressionNode node)
        {
            node.Right.Visit(this);
            node.Left.Visit(this);

            switch (node.BinaryExpressionType)
            {
                case BinaryExpressionType.And:
                    stack.Push((stack.Pop() == "True" && stack.Pop() == "True").ToString());
                    break;
                case BinaryExpressionType.Equality:
                    stack.Push((rowStack.Peek().Data[Convert.ToInt32(stack.Pop())] == stack.Pop()).ToString());
                    break;
                case BinaryExpressionType.GreaterThan:
                    stack.Push((Convert.ToDouble(rowStack.Peek().Data[Convert.ToInt32(stack.Pop())]) > Convert.ToDouble(stack.Pop())).ToString());
                    break;
                case BinaryExpressionType.GreaterThanOrEqual:
                    stack.Push((Convert.ToDouble(rowStack.Peek().Data[Convert.ToInt32(stack.Pop())]) >= Convert.ToDouble(stack.Pop())).ToString());
                    break;
                case BinaryExpressionType.LesserThan:
                    stack.Push((Convert.ToDouble(rowStack.Peek().Data[Convert.ToInt32(stack.Pop())]) < Convert.ToDouble(stack.Pop())).ToString());
                    break;
                case BinaryExpressionType.LesserThanOrEqual:
                    stack.Push((Convert.ToDouble(rowStack.Peek().Data[Convert.ToInt32(stack.Pop())]) <= Convert.ToDouble(stack.Pop())).ToString());
                    break;
                case BinaryExpressionType.NotEqual:
                    stack.Push((rowStack.Peek().Data[Convert.ToInt32(stack.Pop())] != stack.Pop()).ToString());
                    break;
                case BinaryExpressionType.Or:
                    stack.Push((stack.Pop() == "True" || stack.Pop() == "True").ToString());
                    break;
            }
        }
        public void Accept(CreateColumnNode node)
        {
            if (node.Position == -1)
                Database.Select(node.Table).AddColumn(node.Column);
            else
                Database.Select(node.Table).AddColumn(node.Column, node.Position);
        }
        public void Accept(CreateTableNode node)
        {
            Database.Add(new TextualDBTable(node.Table, node.Columns));
        }
        public void Accept(DropColumnNode node)
        {
            Database.Select(node.Table).RemoveColumn(Database.ResolveColumnNumber(node.Table, node.Column));
        }
        public void Accept(DropTableNode node)
        {
            Database.Drop(node.Table);
        }
        public void Accept(IdentifierNode node)
        {
            stack.Push(tableStack.Peek().ResolveColumnNumber(node.Identifier).ToString());
        }
        public void Accept(InsertNode node)
        {
            var table = Database.Select(node.Table);
            if (node.Where == null)
            {
                string[] row = new string[table.Columns.Count];
                foreach (var value in node.Values)
                    row[table.ResolveColumnNumber(value.Column)] = value.Value;
                for (int i = 0; i < row.Length; i++)
                    if (row[i] == null)
                        row[i] = string.Empty;
                table.AddRow(row);
            }
            else
            {
                tableStack.Push(table);
                foreach (var row in table.Rows)
                {
                    rowStack.Push(row);
                    node.Where.Visit(this);
                    if (stack.Pop() == "True")
                        foreach (var value in node.Values)
                            row.Data[table.ResolveColumnNumber(value.Column)] = value.Value;
                    rowStack.Pop();
                }
                tableStack.Pop();
            }
        }
        public void Accept(SelectNode node)
        {
            int column = node.Column != "*" ? Database.ResolveColumnNumber(node.Table, node.Column) : -1;
            if (node.Where == null)
                foreach (var row in Database.Select(node.Table).Rows)
                    response.AppendLine(column != -1 ? row.Data[column] : row.ToString());
            else
            {
                tableStack.Push(Database.Select(node.Table));
                foreach (var row in Database.Select(node.Table).Rows)
                {
                    rowStack.Push(row);
                    node.Where.Visit(this);
                    if (stack.Pop() == "True")
                        response.AppendLine(column != -1 ? row.Data[column] : row.ToString());
                    rowStack.Pop();
                }
                tableStack.Pop();
            }
        }
        public void Accept(StringNode node)
        {
            stack.Push(node.Value);
        }
    }
}
