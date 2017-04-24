using System;
using System.Collections.Generic;

using TextualDB.Components;
using TextualDB.CommandLine.Ast;
using TextualDB.Exceptions;
using TextualDB.Serializer;

namespace TextualDB.CommandLine
{
    public class TextualInterpreter : IVisitor
    {
        private TextualDatabase database;
        private TextualInterpreterResult result;

        public TextualInterpreterResult Execute(AstNode ast, TextualDatabase database)
        {
            this.database = database;
            result = new TextualInterpreterResult();

            ast.Visit(this);

            return result;
        }

        public void Accept(CreateTableNode node)
        {
            List<string> columns = new List<string>();

            foreach (var element in node.Columns.Elements)
            {
                if (!(element is IdentifierNode))
                    throw new CommandLineVisitorException(node.SourceLocation, "Column was not an identifier!");
                columns.Add(((IdentifierNode)element).Identifier);
            }

            database.AddTable(node.Table, columns.ToArray());

            Save();
        }
       
        public void Accept(FilterNode node)
        {
            TextualTable table = new TextualTable(result.TableResult.Name, result.TableResult.Columns.ToArray());

            foreach (var row in result.TableResult.Rows)
            {
                switch (node.FilterType)
                {
                    case TextualFilterType.Equal:
                        if (row.GetValue(node.Column).Value == node.Target)
                            table.AddRow(row);
                        break;
                    case TextualFilterType.NotEqual:
                        if (row.GetValue(node.Column).Value != node.Target)
                            table.AddRow(row);
                        break;
                    case TextualFilterType.Greater:
                        try
                        {
                            if (Convert.ToDouble(row.GetValue(node.Column).Value) > Convert.ToDouble(node.Target))
                                table.AddRow(row);
                        }
                        catch { }
                        break;
                    case TextualFilterType.GreaterOrEqual:
                        try
                        {
                            if (Convert.ToDouble(row.GetValue(node.Column).Value) >= Convert.ToDouble(node.Target))
                                table.AddRow(row);
                        }
                        catch { }
                        break;
                    case TextualFilterType.Lesser:
                        try
                        {
                            if (Convert.ToDouble(row.GetValue(node.Column).Value) < Convert.ToDouble(node.Target))
                                table.AddRow(row);
                        }
                        catch { }
                        break;
                    case TextualFilterType.LesserOrEqual:
                        try
                        {
                            if (Convert.ToDouble(row.GetValue(node.Column).Value) <= Convert.ToDouble(node.Target))
                                table.AddRow(row);
                        }
                        catch { }
                        break;
                }
            }

            result.TableResult = table;
        }

        public void Accept(IdentifierNode node)
        {

        }

        public void Accept(InsertNode node)
        {
            var table = database.GetTable(node.Table);
            TextualRow row = new TextualRow(table);

            foreach (var pair in node.Values)
            {
                if (!table.ContainsColumn(pair.Key))
                    throw new ColumnNotFoundException(pair.Key, table);
                row.AddValue(pair.Key, pair.Value);
            }

            table.AddRow(row, node.Position);

            Save();

            result.TableResult = null;
        }
        
        public void Accept(ListNode node)
        {

        }

        public void Accept(SelectNode node)
        {
            result.TableResult = database.GetTable(node.Table);

            Accept(node.Where);

            List<string> columns = new List<string>();

            foreach (var column in node.Columns.Elements)
            {
                if (!(column is IdentifierNode))
                    throw new CommandLineVisitorException(node.SourceLocation, "Column was not an identifier!");
                columns.Add(((IdentifierNode)column).Identifier);
            }

            result.TableResult = result.TableResult.Select(columns.ToArray());
        }

        public void Accept(ShowNode node)
        {
            result.TextResult.AppendFormat("Tables ({0}):\n", database.FilePath);
            foreach (var table in database.Tables.Keys)
                result.TextResult.AppendLine(table);
        }

        public void Accept(WhereNode node)
        {
            if (result.TableResult == null)
                throw new CommandLineVisitorException(node.SourceLocation, "Unexpected where expression!");
            foreach (var filter in node.Filters)
                filter.Visit(this);
        }

        public void Save()
        {
            new TextualSerializer().SerializeDatabase(database.FilePath, database);
        }
    }
}
