using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.CommandLine.Exceptions;
using TextualDB.CommandLine.Lexer;
using TextualDB.Components;
using TextualDB.Components.Operations;
using TextualDB.Components.Exceptions;

namespace TextualDB.CommandLine.Parser
{
    public class OperationParser
    {
        private TextualDatabase database;
        private List<Token> tokens;
        private int position;

        private bool endOfStream { get { return position >= tokens.Count; } }

        public OperationParser(TextualDatabase database, List<Token> tokens)
        {
            this.database = database;
            this.tokens = tokens;
            position = 0;
        }

        public TextualTable Parse()
        {
            Token first = expectToken(TokenType.Identifier);
            string cmd = first.Value;
            switch (cmd.ToUpper())
            {
                case "CREATE":
                    return parseCreate();
                case "DELETE":
                    return parseDelete();
                case "INSERT":
                    return parseInsert();
                case "SELECT":
                    return parseSelect();
                case "UPDATE":
                    return parseUpdate();
            }
            throw new UnexpectedTokenException(first);
        }

        private TextualTable parseCreate()
        {
            Token second = expectToken(TokenType.Identifier);
            string cmd = second.Value;
            switch (cmd.ToUpper())
            {
                case "COLUMN":
                    return parseCreateColumn();
                case "TABLE":
                    return parseCreateTable();
            }
            throw new UnexpectedTokenException(second);
        }

        private TextualTable parseDelete()
        {
            Token second = expectToken(TokenType.Identifier);
            string cmd = second.Value;
            switch (cmd.ToUpper())
            {
                case "COLUMN":
                    return parseDeleteColumn();
                case "ROW":
                    return parseDeleteRow();
                case "TABLE":
                    return parseDeleteTable();    
            }
            throw new UnexpectedTokenException(second);
        }

        private TextualTable parseCreateColumn()
        {
            // column
            string column = expectToken(TokenType.Identifier).Value;

            expectToken(TokenType.Identifier, "IN");
            // IN table
            string table = expectToken(TokenType.Identifier).Value;
            TextualCreateColumnOperation createColumn = new TextualCreateColumnOperation(database.GetTable(table));

            // AT index
            if (acceptToken(TokenType.Identifier, "AT"))
            {
                int pos = Convert.ToInt32(expectToken(TokenType.Number).Value);
                createColumn.Execute(column, pos);
            }
            else
                createColumn.Execute(column);

            return createColumn.Result;
        }

        private TextualTable parseCreateTable()
        {
            // table
            string table = expectToken(TokenType.Identifier).Value;
            TextualCreateTableOperation createTable = new TextualCreateTableOperation(database, table);

            // WITH column1, column2, ...
            expectToken(TokenType.Identifier, "WITH");
            TextualCreateColumnOperation createColumn = new TextualCreateColumnOperation(createTable.Result);
            foreach (var column in parseIdentifierList())
                createColumn.Execute(column);

            return createTable.Result;
        }

        private TextualTable parseDeleteColumn()
        {
            string table;
            TextualDeleteColumnOperation deleteColumn;

            // FROM table
            if (acceptToken(TokenType.Identifier, "FROM"))
            {
                table = expectToken(TokenType.Identifier).Value;
                deleteColumn = new TextualDeleteColumnOperation(database.GetTable(table));

                // AT index
                expectToken(TokenType.Identifier, "AT");
                int pos = Convert.ToInt32(expectToken(TokenType.Number).Value);

                deleteColumn.Execute(pos);
            }
            // column FROM
            else
            {
                string column = expectToken(TokenType.Identifier).Value;
                expectToken(TokenType.Identifier, "FROM");

                // table
                table = expectToken(TokenType.Identifier).Value;
                deleteColumn = new TextualDeleteColumnOperation(database.GetTable(table));

                deleteColumn.Execute(column);
            }

            return deleteColumn.Result;
        }

        private TextualTable parseDeleteRow()
        {
            // FROM table
            expectToken(TokenType.Identifier, "FROM");
            string table = expectToken(TokenType.Identifier).Value;
            TextualDeleteRowOperation deleteRow = new TextualDeleteRowOperation(database.GetTable(table));

            // AT pos1, pos2, ...
            if (acceptToken(TokenType.Identifier, "AT"))
            {
                int[] indices = parseNumberList();
                foreach (var index in indices)
                    deleteRow.Execute(index);
            }
            else if (acceptToken(TokenType.Identifier, "WHERE"))
            {
                deleteRow.FilterWhereInclusive(parseCondition(deleteRow));
                while (matchToken(TokenType.Comparison))
                {
                    // AND condition1
                    if (acceptToken(TokenType.Comparison, "AND"))
                        deleteRow.FilterWhereExclusive(parseCondition(deleteRow));
                    // OR condition1
                    else if (acceptToken(TokenType.Comparison, "OR"))
                        deleteRow.FilterWhereInclusive(parseCondition(deleteRow));
                    else
                        throw new UnexpectedTokenException(tokens[position]);
                }
                deleteRow.Execute();
            }
            else
                throw new UnexpectedTokenException(tokens[position]);

            return deleteRow.Result;
        }

        private TextualTable parseDeleteTable()
        {
            // table
            string table = expectToken(TokenType.Identifier).Value;
            TextualDeleteTableOperation deleteTable = new TextualDeleteTableOperation(database);
            deleteTable.Execute(table);

            return deleteTable.Result;
        }
        
        private TextualTable parseInsert()
        {
            // INTO table
            expectToken(TokenType.Identifier, "INTO");
            string table = expectToken(TokenType.Identifier).Value;
            TextualInsertOperation insert = new TextualInsertOperation(database.GetTable(table));

            // AT 1, 2, ...
            int[] indices = acceptToken(TokenType.Identifier, "AT") ? parseNumberList() : new int[] { -1 };

            // VALUES column1="val1", column2=3, ...
            expectToken(TokenType.Identifier, "VALUES");
            do
            {
                string column = expectToken(TokenType.Identifier).Value;
                expectToken(TokenType.Comparison, "=");
                object val;
                if (matchToken(TokenType.Number))
                    val = Convert.ToDouble(expectToken(TokenType.Number).Value);
                else if (matchToken(TokenType.String))
                    val = expectToken(TokenType.String).Value;
                else
                    throw new UnexpectedTokenException(tokens[position]);
                insert.BuildRow(column, val);
            } while (acceptToken(TokenType.Comma));

            foreach (var index in indices)
                insert.Execute(index);

            return insert.Result;
        }

        private TextualTable parseSelect()
        {
            // column1, column2, ... FROM table
            string[] columns = parseIdentifierList();
            expectToken(TokenType.Identifier, "FROM");
            string table = expectToken(TokenType.Identifier).Value;
            TextualSelectOperation select = new TextualSelectOperation(database.GetTable(table), columns);

            // AT index1, index2, ...
            if (acceptToken(TokenType.Identifier, "AT"))
            {
                int[] indices = parseNumberList();
                select.FilterWhereExclusive(indices);
            }
            // WHERE condition1 AND/OR condition2 AND/OR ...
            else if (acceptToken(TokenType.Identifier, "WHERE"))
            {
                select.FilterWhereExclusive(parseCondition(select));
                while (matchToken(TokenType.Comparison))
                {
                    // AND condition1
                    if (acceptToken(TokenType.Comparison, "AND"))
                        select.FilterWhereExclusive(parseCondition(select));
                    // OR condition1
                    else if (acceptToken(TokenType.Comparison, "OR"))
                        select.FilterWhereInclusive(parseCondition(select));
                    else
                        throw new UnexpectedTokenException(tokens[position]);
                }
            }
            
            select.Execute();
            return select.Result;
        }

        private TextualTable parseUpdate()
        {
            // table VALUES
            string table = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "VALUES");

            var values = new Dictionary<string, object>();
            // column1=val1, column2="val2", ...
            do
            {
                string column = expectToken(TokenType.Identifier).Value;
                expectToken(TokenType.Comparison, "=");
                object val;
                if (matchToken(TokenType.Number))
                    val = Convert.ToDouble(expectToken(TokenType.Number).Value);
                else if (matchToken(TokenType.String))
                    val = expectToken(TokenType.String).Value;
                else
                    throw new UnexpectedTokenException(tokens[position]);

                values.Add(column, val);
            } while (acceptToken(TokenType.Comma));
            TextualUpdateOperation update = new TextualUpdateOperation(database.GetTable(table), values.Keys.ToArray(), values.Values.ToArray());
            
            // AT pos1, pos2, ...
            if (acceptToken(TokenType.Identifier, "AT"))
                foreach (var index in parseNumberList())
                    update.Execute(index);
            // WHERE condition1, condition2, ...
            else if (acceptToken(TokenType.Identifier, "WHERE"))
            {
                update.FilterWhereExclusive(parseCondition(update));
                while (matchToken(TokenType.Comparison))
                {
                    // AND condition1
                    if (acceptToken(TokenType.Comparison, "AND"))
                        update.FilterWhereExclusive(parseCondition(update));
                    // OR condition1
                    else if (acceptToken(TokenType.Comparison, "OR"))
                        update.FilterWhereInclusive(parseCondition(update));
                    else
                        throw new UnexpectedTokenException(tokens[position]);
                }
                update.Execute();
            }
            else
                throw new UnexpectedTokenException(tokens[position]);

            return update.Result;
        }

        private string[] parseIdentifierList()
        {
            if (acceptToken(TokenType.Identifier, "*"))
                return new string[0];

            List<string> strings = new List<string>();
            do
            {
                strings.Add(expectToken(TokenType.Identifier).Value);
            }
            while (acceptToken(TokenType.Comma));
            return strings.ToArray();
        }

        private TextualWhereCondition parseCondition(TextualOperation operation)
        {
            // column1
            string column = expectToken(TokenType.Identifier).Value;
            // =
            string op = expectToken(TokenType.Comparison).Value;
            object val;
            // 1
            if (matchToken(TokenType.Number))
                val = Convert.ToDouble(expectToken(TokenType.Number).Value);
            // "val1"
            else
                val = expectToken(TokenType.String).Value;
            return new TextualWhereCondition(TextualWhereCondition.ParseWhereOperation(operation, op), column, val);
        }

        private int[] parseNumberList()
        {
            List<int> nums = new List<int>();
            do
            {
                nums.Add(Convert.ToInt32(expectToken(TokenType.Number).Value));
            }
            while (acceptToken(TokenType.Comma));
            return nums.ToArray();
        }

        private bool matchToken(TokenType tokenType)
        {
            return !endOfStream && tokens[position].TokenType == tokenType;
        }
        private bool matchToken(TokenType tokenType, string value)
        {
            return !endOfStream && tokens[position].TokenType == tokenType && tokens[position].Value.ToUpper() == value.ToUpper();
        }

        private bool acceptToken(TokenType tokenType)
        {
            if (matchToken(tokenType))
            {
                position++;
                return true;
            }
            return false;
        }
        private bool acceptToken(TokenType tokenType, string value)
        {
            if (matchToken(tokenType, value))
            {
                position++;
                return true;
            }
            return false;
        }

        private Token expectToken(TokenType tokenType)
        {
            if (matchToken(tokenType))
                return tokens[position++];
            throw new ExpectedTokenException(tokens[position].SourceLocation, tokenType);
        }
        private Token expectToken(TokenType tokenType, string value)
        {
            if (matchToken(tokenType, value))
                return tokens[position++];
            throw new ExpectedTokenException(tokens[position].SourceLocation, tokenType, value);
        }
    }
}
