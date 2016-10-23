using System;
using System.Collections.Generic;

using TextualDBD.Exceptions;
using TextualDBD.Interpreter.Ast;

namespace TextualDBD.Interpreter
{
    public class Parser
    {
        public List<Token> Tokens { get; private set; }
        public bool Eof { get { return position >= Tokens.Count; } }
        private int position;

        public AstNode Parse(List<Token> tokens)
        {
            Tokens = tokens;
            position = 0;
            return parseStatement();
        }

        private AstNode parseStatement()
        {
            if (matchToken(TokenType.Identifier, "create"))
                return parseCreate();
            else if (matchToken(TokenType.Identifier, "drop"))
                return parseDrop();
            else if (matchToken(TokenType.Identifier, "insert"))
                return parseInsert();
            else if (matchToken(TokenType.Identifier, "select"))
                return parseSelect();
            else if (matchToken(TokenType.Identifier, "show"))
                return parseShow();
            else
                return parseExpression();
        }

        private AstNode parseCreate()
        {
            expectToken(TokenType.Identifier, "create");
            if (acceptToken(TokenType.Identifier, "table"))
            {
                string table = expectToken(TokenType.Identifier).Value;
                List<string> columns = new List<string>();
                expectToken(TokenType.Identifier, "with");
                while (!Eof)
                {
                    columns.Add(expectToken(TokenType.Identifier).Value);
                    acceptToken(TokenType.Comma);
                }
                return new CreateTableNode(table, columns);
            }
            else if (acceptToken(TokenType.Identifier, "column"))
            {
                string column = expectToken(TokenType.Identifier).Value;
                int pos = -1;
                if (acceptToken(TokenType.Identifier, "at"))
                    pos = Convert.ToInt32(expectToken(TokenType.Identifier).Value);
                expectToken(TokenType.Identifier, "in");
                return new CreateColumnNode(expectToken(TokenType.Identifier).Value, column, pos);
            }
            expectToken(TokenType.Identifier, "table or column");
            return null;
        }
        private AstNode parseDrop()
        {
            expectToken(TokenType.Identifier, "drop");
            if (acceptToken(TokenType.Identifier, "table"))
                return new DropTableNode(expectToken(TokenType.Identifier).Value);
            else if (acceptToken(TokenType.Identifier, "column"))
            {
                string column = expectToken(TokenType.Identifier).Value;
                expectToken(TokenType.Identifier, "in");
                return new DropColumnNode(expectToken(TokenType.Identifier).Value, column);
            }
            expectToken(TokenType.Identifier, "table or column");
            return null;
        }
        private InsertNode parseInsert()
        {
            expectToken(TokenType.Identifier, "insert");
            acceptToken(TokenType.Identifier, "into");
            string table = expectToken(TokenType.Identifier).Value;
            List<InsertValue> values = new List<InsertValue>();
            while (!Eof && !matchToken(TokenType.Identifier, "where"))
            {
                string column = expectToken(TokenType.Identifier).Value;
                expectToken(TokenType.Comparison, "==");
                string value = Tokens[position++].Value;
                values.Add(new InsertValue(column, value));
                acceptToken(TokenType.Comma);
            }
            AstNode where = null;
            if (acceptToken(TokenType.Identifier, "where"))
                where = parseExpression();
            return new InsertNode(table, values, where);
        }
        private SelectNode parseSelect()
        {
            expectToken(TokenType.Identifier, "select");
            string column = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Identifier, "from");
            string table = expectToken(TokenType.Identifier).Value;
            if (acceptToken(TokenType.Identifier, "where"))
                return new SelectNode(column, table, parseExpression());
            return new SelectNode(column, table, null);
        }
        private AstNode parseShow()
        {
            expectToken(TokenType.Identifier, "show");
            if (acceptToken(TokenType.Identifier, "columns"))
            {
                acceptToken(TokenType.Identifier, "in");
                return new ShowColumnsNode(expectToken(TokenType.Identifier).Value);
            }
            else if (acceptToken(TokenType.Identifier, "tables"))
                return new ShowTablesNode();
            expectToken(TokenType.Identifier, "columns or tables");
            return null;
        }

        private AstNode parseExpression()
        {
            return parseOr();
        }

        private AstNode parseOr()
        {
            AstNode left = parseAnd();
            while (acceptToken(TokenType.Operation, "||"))
                left = new BinaryExpressionNode(BinaryExpressionType.Or, left, parseOr());
            return left;
        }

        private AstNode parseAnd()
        {
            AstNode left = parseComparison();
            while (acceptToken(TokenType.Operation, "&&"))
                left = new BinaryExpressionNode(BinaryExpressionType.And, left, parseAnd());
            return left;
        }

        private AstNode parseComparison()
        {
            AstNode left = parseTerm();
            while (matchToken(TokenType.Comparison))
            {
                switch (Tokens[position].Value)
                {
                    case ">":
                        expectToken(TokenType.Comparison);
                        return new BinaryExpressionNode(BinaryExpressionType.GreaterThan, left, parseComparison());
                    case ">=":
                        expectToken(TokenType.Comparison);
                        return new BinaryExpressionNode(BinaryExpressionType.GreaterThanOrEqual, left, parseComparison());
                    case "<":
                        expectToken(TokenType.Comparison);
                        return new BinaryExpressionNode(BinaryExpressionType.LesserThan, left, parseComparison());
                    case "<=":
                        expectToken(TokenType.Comparison);
                        return new BinaryExpressionNode(BinaryExpressionType.LesserThanOrEqual, left, parseComparison());
                    case "==":
                        expectToken(TokenType.Comparison);
                        return new BinaryExpressionNode(BinaryExpressionType.Equality, left, parseComparison());
                    case "!=":
                        expectToken(TokenType.Comparison);
                        return new BinaryExpressionNode(BinaryExpressionType.NotEqual, left, parseComparison());
                    default:
                        break;
                }
                break;
            }
            return left;
        }

        private AstNode parseTerm()
        {
            if (matchToken(TokenType.Identifier))
                return new IdentifierNode(expectToken(TokenType.Identifier).Value);
            else if (matchToken(TokenType.String))
                return new StringNode(expectToken(TokenType.String).Value);
            else if (acceptToken(TokenType.OpenParentheses))
            {
                AstNode expr = parseExpression();
                expectToken(TokenType.CloseParentheses);
                return expr;
            }
            throw new ParserUnexpectedTokenException(Tokens[position]);
        }

        private bool matchToken(TokenType tokenType)
        {
            if (Eof)
                return false;
            return Tokens[position].TokenType == tokenType;
        }
        private bool matchToken(TokenType tokenType, string value)
        {
            if (Eof)
                return false;
            return Tokens[position].TokenType == tokenType && Tokens[position].Value == value;
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
                return Tokens[position++];
            throw new ParserExpectedTokenException(tokenType, Tokens[position++]);
        }
        private Token expectToken(TokenType tokenType, string value)
        {
            if (matchToken(tokenType, value))
                return Tokens[position++];
            throw new ParserExpectedTokenException(tokenType, value, Tokens[position++]);
        }
    }
}