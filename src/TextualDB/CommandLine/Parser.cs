using System;
using System.Collections.Generic;

using TextualDB.CommandLine.Ast;
using TextualDB.Deserializer.Lexer;
using TextualDB.Exceptions;

namespace TextualDB.CommandLine
{
    public class Parser
    {
        private List<Token> tokens;
        private int position;

        private Token currentToken { get { return eof ? tokens[position - 1] : tokens[position]; } }
        private bool eof { get { return position >= tokens.Count; } }

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            position = 0;
        }

        public AstNode Parse()
        {
            return parseStatement();
        }

        private AstNode parseStatement()
        {
            var location = currentToken.SourceLocation;
            if (matchToken(TokenType.Identifier, "create"))
                return parseCreate();
            else if (matchToken(TokenType.Identifier, "delete"))
                return parseDelete();
            else if (matchToken(TokenType.Identifier, "insert"))
                return parseInsert();
            else if (matchToken(TokenType.Identifier, "rename"))
                return parseRename();
            else if (matchToken(TokenType.Identifier, "show"))
                return parseShow();
            else if (matchToken(TokenType.Identifier, "select"))
                return parseSelect();
            else if (matchToken(TokenType.Identifier, "update"))
                return parseUpdate();
            else if (matchToken(TokenType.Identifier))
                return new IdentifierNode(currentToken.SourceLocation, expectToken(TokenType.Identifier).Value);
            else if (matchToken(TokenType.Number))
                return new NumberNode(currentToken.SourceLocation, Convert.ToInt32(expectToken(TokenType.Number).Value));
            else
                throw new CommandLineParseException(currentToken.SourceLocation, "Unexpected token of type {0} with value {1}!", currentToken.Value, currentToken.TokenType);
        }

        private AstNode parseCreate()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "create");
            if (matchToken(TokenType.Identifier, "column"))
                return parseCreateColumn();
            else if (matchToken(TokenType.Identifier, "table"))
                return parseCreateTable();
            else
                throw new CommandLineParseException(location, "Expected token of type Identifier with value 'column' or 'table'. Got {0} with value '{1}'!", currentToken.TokenType, currentToken.Value);
        }
        private CreateColumnNode parseCreateColumn()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "column");
            string column = expectToken(TokenType.Identifier).Value;
            int position = -1;
            if (acceptToken(TokenType.Identifier, "at"))
                position = Convert.ToInt32(expectToken(TokenType.Number).Value);
            expectToken(TokenType.Identifier, "in");
            string table = expectToken(TokenType.Identifier).Value;

            return new CreateColumnNode(location, column, table, position);
        }
        private CreateTableNode parseCreateTable()
        {
            var location = currentToken.SourceLocation;
            
            expectToken(TokenType.Identifier, "table");
            string table = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "with");
            ListNode columns = parseList();

            return new CreateTableNode(location, table, columns);
        }

        private AstNode parseDelete()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "delete");

            if (matchToken(TokenType.Identifier, "column"))
                return parseDeleteColumn();
            else if (matchToken(TokenType.Identifier, "row"))
                return parseDeleteRow();
            else if (matchToken(TokenType.Identifier, "table"))
                return parseDeleteTable();
            else
                throw new CommandLineParseException(location, "Expected token of type Identifier with value 'column' or 'table'. Got {0} with value '{1}'!", currentToken.TokenType, currentToken.Value);
        }
        private DeleteColumnNode parseDeleteColumn()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "column");
            string column = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "from");
            string table = expectToken(TokenType.Identifier).Value;

            return new DeleteColumnNode(location, column, table);
        }
        private DeleteRowNode parseDeleteRow()
        {
            expectToken(TokenType.Identifier, "row");
            expectToken(TokenType.Identifier, "from");
            string table = expectToken(TokenType.Identifier).Value;

            var location = currentToken.SourceLocation;
            AstNode locator;
            if (acceptToken(TokenType.Identifier, "at"))
                locator = parseList();
            else if (matchToken(TokenType.Identifier, "where"))
                locator = parseWhere();
            else
                throw new CommandLineParseException(location, "Expected token of type Identifier with value 'at' or 'where'. Got {0} with value '{1}'!", currentToken.TokenType, currentToken.Value);

            if (locator is WhereNode)
                return new DeleteRowNode(location, table, locator as WhereNode);
            else
                return new DeleteRowNode(location, table, locator as ListNode);
        }
        private DeleteTableNode parseDeleteTable()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "table");
            string table = expectToken(TokenType.Identifier).Value;

            return new DeleteTableNode(location, table);
        }

        private FilterNode parseFilter()
        {
            var location = currentToken.SourceLocation;

            string column = expectToken(TokenType.Identifier).Value;

            TextualFilterType filterType;

            switch (expectToken(TokenType.Comparison).Value)
            {
                case "=":
                    filterType = TextualFilterType.Equal;
                    break;
                case "!=":
                    filterType = TextualFilterType.NotEqual;
                    break;
                case "<":
                    filterType = TextualFilterType.Lesser;
                    break;
                case "<=":
                    filterType = TextualFilterType.LesserOrEqual;
                    break;
                case ">":
                    filterType = TextualFilterType.Greater;
                    break;
                case ">=":
                    filterType = TextualFilterType.GreaterOrEqual;
                    break;
                default:
                    throw new CommandLineParseException(location, "Unknown operation {0}!", tokens[position - 1].Value);
            }

            string value;
            if (matchToken(TokenType.String))
                value = expectToken(TokenType.String).Value;
            else if (matchToken(TokenType.Number))
                value = expectToken(TokenType.Number).Value;
            else
                throw new CommandLineParseException(location, "Unexpected token of type {0} with value {1}!", currentToken.Value, currentToken.TokenType);

            return new FilterNode(location, column, value, filterType);
        }

        private InsertNode parseInsert()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "insert");
            expectToken(TokenType.Identifier, "into");

            string table = expectToken(TokenType.Identifier).Value;

            int pos = -1;

            if (acceptToken(TokenType.Identifier, "at"))
                pos = Convert.ToInt32(expectToken(TokenType.Number).Value);

            Dictionary<string, string> values = new Dictionary<string, string>();

            if (acceptToken(TokenType.Identifier, "values"))
            {
                do
                {
                    string key = expectToken(TokenType.Identifier).Value;
                    expectToken(TokenType.Comparison, "=");
                    string val = expectToken(TokenType.String).Value;

                    values.Add(key, val);
                }
                while (acceptToken(TokenType.Comma));
            }

            return new InsertNode(location, table, values, pos);
        }

        private ListNode parseList()
        {
            var location = currentToken.SourceLocation;
            List<AstNode> elements = new List<AstNode>();

            do
            {
                elements.Add(Parse());
            } while (acceptToken(TokenType.Comma));

            return new ListNode(location, elements);
        }

        private AstNode parseRename()
        {
            expectToken(TokenType.Identifier, "rename");
            var location = currentToken.SourceLocation;

            if (matchToken(TokenType.Identifier, "column"))
                return parseRenameColumn();
            else if (matchToken(TokenType.Identifier, "table"))
                return parseRenameTable();
            else
                throw new CommandLineParseException(location, "Expected token of type Identifier with value 'column' or 'table'. Got {0} with value '{1}'!", currentToken.TokenType, currentToken.Value);
        }
        private RenameColumnNode parseRenameColumn()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "column");
            string oldName = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "to");
            string newName = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "in");
            string table = expectToken(TokenType.Identifier).Value;

            return new RenameColumnNode(location, table, oldName, newName);
        }
        private RenameTableNode parseRenameTable()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "table");
            string oldName = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "to");
            string newName = expectToken(TokenType.Identifier).Value;

            return new RenameTableNode(location, oldName, newName);
        }

        private SelectNode parseSelect()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "select");

            ListNode columns;
            if (acceptToken(TokenType.Asterisk))
                columns = new ListNode(location, new AstNode[0]);
            else
                columns = parseList();

            expectToken(TokenType.Identifier, "from");

            string table = expectToken(TokenType.Identifier).Value;

            AstNode locator;
            if (matchToken(TokenType.Identifier, "where"))
                locator = parseWhere();
            else if (acceptToken(TokenType.Identifier, "at"))
                locator = parseList();
            else
                locator = new WhereNode(location, new FilterNode[0]);

            if (locator is ListNode)
                return new SelectNode(location, columns, table, locator as ListNode);
            else
                return new SelectNode(location, columns, table, locator as WhereNode);
        }

        private AstNode parseShow()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "show");

            if (matchToken(TokenType.Identifier, "columns"))
                return parseShowColumns();
            else if (matchToken(TokenType.Identifier, "tables"))
                return parseShowTables();
            else
                throw new CommandLineParseException(location, "Expected token of type Identifier with value 'column' or 'table'. Got {0} with value '{1}'!", currentToken.TokenType, currentToken.Value);
        }

        private ShowColumnsNode parseShowColumns()
        {
            expectToken(TokenType.Identifier, "columns");
            expectToken(TokenType.Identifier, "in");
            var location = currentToken.SourceLocation;
            string table = expectToken(TokenType.Identifier).Value;

            return new ShowColumnsNode(location, table);
        }

        private ShowTablesNode parseShowTables()
        {
            var location = currentToken.SourceLocation;
            expectToken(TokenType.Identifier, "tables");

            return new ShowTablesNode(location);
        }

        private UpdateNode parseUpdate()
        {
            expectToken(TokenType.Identifier, "update");
            string table = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "values");

            Dictionary<string, string> values = new Dictionary<string, string>();
            do
            {
                string key = expectToken(TokenType.Identifier).Value;
                expectToken(TokenType.Comparison, "=");
                string val = expectToken(TokenType.String).Value;

                values.Add(key, val);
            }
            while (acceptToken(TokenType.Comma));

            var location = currentToken.SourceLocation;
            AstNode locator;
            if (acceptToken(TokenType.Identifier, "at"))
                locator = parseList();
            else if (matchToken(TokenType.Identifier, "where"))
                locator = parseWhere();
            else
                throw new CommandLineParseException(location, "Expected token of type Identifier with value 'at' or 'where'. Got {0} with value '{1}'!", currentToken.TokenType, currentToken.Value);

            if (locator is WhereNode)
                return new UpdateNode(location, table, values, locator as WhereNode);
            else
                return new UpdateNode(location, table, values, locator as ListNode);
        }

        private WhereNode parseWhere()
        {
            expectToken(TokenType.Identifier, "where");

            var location = currentToken.SourceLocation;

            List<FilterNode> filters = new List<FilterNode>();

            do
            {
                filters.Add(parseFilter());
            } while (acceptToken(TokenType.Comma));

            return new WhereNode(location, filters);
        }

        private bool matchToken(TokenType tokenType)
        {
            if (eof)
                return false;
            return currentToken.TokenType == tokenType;
        }
        private bool matchToken(TokenType tokenType, string val)
        {
            if (eof)
                return false;
            return currentToken.TokenType == tokenType && currentToken.Value == val;
        }

        private bool acceptToken(TokenType tokenType)
        {
            bool ret = matchToken(tokenType);

            if (ret)
                position++;

            return ret;
        }
        private bool acceptToken(TokenType tokenType, string val)
        {
            bool ret = matchToken(tokenType, val);

            if (ret)
                position++;

            return ret;
        }

        private Token expectToken(TokenType tokenType)
        {
            if (!matchToken(tokenType))
                throw new CommandLineParseException(currentToken.SourceLocation, "Expected token of type {0}, got {1} with value '{2}'", tokenType, currentToken.TokenType, currentToken.Value);
            try
            {
                return currentToken;
            }
            finally
            {
                position++;
            }
        }

        private Token expectToken(TokenType tokenType, string val)
        {
            if (!matchToken(tokenType, val))
                throw new CommandLineParseException(currentToken.SourceLocation, "Expected token of type {0} with value '{1}', got {2} with value '{3}'!", tokenType, val, currentToken.TokenType, currentToken.Value);
            try
            {
                return currentToken;
            }
            finally
            {
                position++;
            }
        }
    }
}
