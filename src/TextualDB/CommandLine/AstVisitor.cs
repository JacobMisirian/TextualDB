using System;
using System.Collections.Generic;

using TextualDB.Components;
using TextualDB.CommandLine.Ast;
using TextualDB.Exceptions;
using TextualDB.Serializer;

namespace TextualDB.CommandLine
{
    public class AstVisitor : IVisitor
    {
        private TextualDatabase database;

        private TextualTable tableResult;

        public AstVisitor(AstNode ast, TextualDatabase database)
        {
            this.database = database;

            ast.Visit(this);

            Console.WriteLine(tableResult.ToString());
        }

        public void Accept(FilterNode node)
        {
            TextualTable table = new TextualTable(tableResult.Name, tableResult.Columns.ToArray());

            foreach (var row in tableResult.Rows)
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

            tableResult = table;
        }

        public void Accept(IdentifierNode node)
        {

        }

        public void Accept(InsertNode node)
        {
            tableResult = database.GetTable(node.Table);
            TextualRow row = new TextualRow(tableResult);

            foreach (var pair in node.Values)
            {
                if (!tableResult.ContainsColumn(pair.Key))
                    throw new ColumnNotFoundException(pair.Key, tableResult);
                row.AddValue(pair.Key, pair.Value);
            }

            tableResult.AddRow(row, node.Position);

            Save();
        }
        
        public void Accept(ListNode node)
        {

        }

        public void Accept(SelectNode node)
        {
            tableResult = database.GetTable(node.Table);

            Accept(node.Where);

            List<string> columns = new List<string>();

            foreach (var column in node.Columns.Elements)
            {
                if (!(column is IdentifierNode))
                    throw new CommandLineVisitorException(node.SourceLocation, "Column was not an identifier!");
                columns.Add(((IdentifierNode)column).Identifier);
            }

            tableResult = tableResult.Select(columns.ToArray());
        }

        public void Accept(WhereNode node)
        {
            if (tableResult == null)
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
