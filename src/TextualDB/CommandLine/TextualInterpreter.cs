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

        public void Accept(CreateColumnNode node)
        {
            var table = database.GetTable(node.Table);

            table.AddColumn(node.Column, node.Position);

            Save();
        }

        public void Accept(CreateTableNode node)
        {
            List<string> columns = new List<string>();

            foreach (var element in node.Columns.Elements)
            {
                if (!(element is IdentifierNode))
                    throw new CommandLineInterpreterException(node.SourceLocation, "Column was not an identifier!");
                columns.Add(((IdentifierNode)element).Identifier);
            }

            database.AddTable(node.Table, columns.ToArray());

            Save();
        }

        public void Accept(DeleteColumnNode node)
        {
            var table = database.GetTable(node.Table);
            table.RemoveColumn(node.Column);

            Save();
        }

        public void Accept(DeleteRowNode node)
        {
            var table = database.GetTable(node.Table);
            if (node.Positions != null)
            {
                List<TextualRow> rows = new List<TextualRow>();
                foreach (var pos in node.Positions.Elements)
                {
                    if (!(pos is NumberNode))
                        throw new CommandLineInterpreterException(node.SourceLocation, "Position was not a number!");
                    int position = ((NumberNode)pos).Number;

                    rows.Add(table.GetRow(position));
                }

                foreach (var row in rows)
                    table.RemoveRow(row);
            }
            else
            {
                result.TableResult = table;
                Accept(node.Where);

                foreach (var row in result.TableResult.Rows)
                    table.RemoveRow(row);
            }

            Save();

            result.TableResult = null;
        }

        public void Accept(DeleteTableNode node)
        {
            database.RemoveTable(node.Table);

            Save();
        }
       
        public void Accept(FilterNode node)
        {
            TextualTable table = new TextualTable(result.TableResult.Name, result.TableResult.Columns.ToArray());

            foreach (var row in result.TableResult.Rows)
            {
                switch (node.FilterType)
                {
                    case TextualFilterType.Contains:
                        if (row.GetValue(node.Column).Value.Contains(node.Target))
                            table.AddRow(row);
                        break;
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
            List<int> nums = new List<int>();
            foreach (var position in node.Elements)
            {
                if (!(position is NumberNode))
                    throw new CommandLineInterpreterException(node.SourceLocation, "Position was not a number!");
                nums.Add(Convert.ToInt32(((NumberNode)position).Number));
            }
            
            var table = new TextualTable(result.TableResult.Name, result.TableResult.Columns.ToArray());
            
            foreach (var num in nums)
                table.AddRow(result.TableResult.GetRow(num).ChangeOwner(table));
                
            result.TableResult = table;
        }

        public void Accept(NumberNode node)
        {

        }

        public void Accept(RenameColumnNode node)
        {
            var table = database.GetTable(node.Table);

            table.ChangeColumnName(node.OldName, node.NewName);

            Save();
        }

        public void Accept(RenameTableNode node)
        {
            var table = database.GetTable(node.OldName);
            table.Name = node.NewName;
            
            database.Tables.Remove(node.OldName);
            database.Tables.Add(node.NewName, table);

            Save();
        }

        public void Accept(SelectNode node)
        {
            result.TableResult = database.GetTable(node.Table);

            if (node.Positions == null)
                Accept(node.Where);
            else
                Accept(node.Positions);

            List<string> columns = new List<string>();

            foreach (var column in node.Columns.Elements)
            {
                if (!(column is IdentifierNode))
                    throw new CommandLineInterpreterException(node.SourceLocation, "Column was not an identifier!");
                columns.Add(((IdentifierNode)column).Identifier);
            }

            result.TableResult = result.TableResult.Select(columns.ToArray());
        }

        public void Accept(ShowColumnsNode node)
        {
            var table = database.GetTable(node.Table);
            result.TableResult = new TextualTable(node.Table, "column");

            foreach (var column in table.Columns)
                result.TableResult.AddRow(-1, column);
        }

        public void Accept(ShowTablesNode node)
        {
            result.TableResult = new TextualTable(database.FilePath, "table");

            foreach (var table in database.Tables.Keys)
                result.TableResult.AddRow(-1, table);
        }

        public void Accept(UpdateNode node)
        {
            var table = database.GetTable(node.Table);

            if (node.Positions != null)
            {
                foreach (var pos in node.Positions.Elements)
                {
                    if (!(pos is NumberNode))
                        throw new CommandLineInterpreterException(node.SourceLocation, "Position was not a number!");
                    int position = ((NumberNode)pos).Number;
                    var row = table.GetRow(position);

                    foreach (var pair in node.Values)
                        row.AddValue(pair.Key, pair.Value);
                }
            }
            else
            {
                result.TableResult = table;
                Accept(node.Where);

                foreach (var row in result.TableResult.Rows)
                {
                    foreach (var pair in node.Values)
                        row.AddValue(pair.Key, pair.Value);
                }
            }

            Save();

            result.TableResult = null;
        }

        public void Accept(WhereNode node)
        {
            if (result.TableResult == null)
                throw new CommandLineInterpreterException(node.SourceLocation, "Unexpected where expression!");
            foreach (var filter in node.Filters)
                filter.Visit(this);
        }

        public void Save()
        {
            new TextualSerializer().SerializeDatabase(database.FilePath, database);
        }
    }
}
