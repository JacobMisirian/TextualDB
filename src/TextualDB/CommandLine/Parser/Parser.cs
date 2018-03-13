using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.CommandLine.Exceptions;
using TextualDB.CommandLine.Lexer;
using TextualDB.Components;
using TextualDB.Components.Operations;

namespace TextualDB.CommandLine.Parser
{
    public class Parser
    {
        private TextualDatabase database;
        private List<Token> tokens;
        private int position;

        private bool endOfStream { get { return position >= tokens.Count; } }

        public Parser(TextualDatabase database, List<Token> tokens)
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
                case "SELECT":
                    return parseSelect();
            }
            throw new UnexpectedTokenException(first.SourceLocation, first.TokenType, first.Value);
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

        private TextualWhereCondition parseCondition(TextualOperation operation)
        {
            string column = expectToken(TokenType.Identifier).Value;
            string op = expectToken(TokenType.Comparison).Value;
            object val;
            if (matchToken(TokenType.Number))
                val = Convert.ToDouble(expectToken(TokenType.Number).Value);
            else
                val = expectToken(TokenType.String).Value;
            return new TextualWhereCondition(TextualWhereCondition.ParseWhereOperation(operation, op), column, val);
        }

        private TextualTable parseSelect()
        {
            // column1, column2, ... FROM table
            string[] columns = parseIdentifierList();
            expectToken(TokenType.Identifier, "FROM");
            string table = expectToken(TokenType.Identifier).Value;
            TextualSelectOperation select = new TextualSelectOperation(database.GetTable(table), columns);

            // FROM table AT index1, index2, ...
            if (acceptToken(TokenType.Identifier, "AT"))
            {
                int[] indices = parseNumberList();
                select.FilterWhereExclusive(indices);
            }
            // FROM table WHERE condition1 AND/OR condition2 AND/OR ...
            else if (acceptToken(TokenType.Identifier, "WHERE"))
            {
                select.FilterWhereExclusive(parseCondition(select));
                while (matchToken(TokenType.Comparison))
                {
                    if (acceptToken(TokenType.Comparison, "AND"))
                        select.FilterWhereExclusive(parseCondition(select));
                    else if (acceptToken(TokenType.Comparison, "OR"))
                        select.FilterWhereInclusive(parseCondition(select));
                    else
                        throw new UnexpectedTokenException(tokens[position].SourceLocation, tokens[position].TokenType, tokens[position].Value);
                }
            }
            
            select.Execute();
            return select.Result;
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
