﻿using System;
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

        private Token currentToken { get { return tokens[position]; } }
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
                return parseCreateTable();
            else if (matchToken(TokenType.Identifier, "insert"))
                return parseInsert();
            else if (acceptToken(TokenType.Identifier, "show"))
                return new ShowNode(location);
            else if (matchToken(TokenType.Identifier, "select"))
                return parseSelect();
            else if (matchToken(TokenType.Identifier))
                return new IdentifierNode(currentToken.SourceLocation, expectToken(TokenType.Identifier).Value);
            else
                throw new CommandLineParseException(currentToken.SourceLocation, "Unexpected token of type {0} with value {1}!", currentToken.Value, currentToken.TokenType);
        }

        private CreateTableNode parseCreateTable()
        {
            var location = currentToken.SourceLocation;

            expectToken(TokenType.Identifier, "create");
            expectToken(TokenType.Identifier, "table");
            string table = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "with");
            ListNode columns = parseList();

            return new CreateTableNode(location, table, columns);
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

            WhereNode where;
            if (matchToken(TokenType.Identifier, "where"))
                where = parseWhere();
            else
                where = new WhereNode(location, new FilterNode[0]);

            return new SelectNode(location, columns, table, where);
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
