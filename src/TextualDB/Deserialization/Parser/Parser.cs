using System;
using System.Collections.Generic;

using TextualDB.Components;
using TextualDB.Deserialization.Exceptions;
using TextualDB.Deserialization.Lexer;

namespace TextualDB.Deserialization.Parser
{
    public class Parser
    {
        private List<Token> tokens;
        private int position;
        private bool endOfStream { get { return position >= tokens.Count; } }

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            position = 0;
        }

        public TextualDatabase ParseDatabase(string name)
        {
            TextualDatabase db = new TextualDatabase(name);

            while (!endOfStream)
                db.AddTable(parseTable(db));

            return db;
        }

        private TextualTable parseTable(TextualDatabase db)
        {
            // name:
            string name = expectToken(TokenType.Identifier).Value;
            expectToken(TokenType.Colon);

            TextualTable table = new TextualTable(db, name);

            // | column1 | column2 | column3 |
            expectToken(TokenType.Pipe);
            while (!matchToken(TokenType.Dash))
            {
                table.Columns.Add(expectToken(TokenType.Identifier).Value);
                expectToken(TokenType.Pipe);
            }

            // -------------
            while (acceptToken(TokenType.Dash)) ;

            // Rows
            while (!matchToken(TokenType.Question))
                table.AddRow(parseRow(table));

            // ?
            expectToken(TokenType.Question);

            return table;
        }

        private TextualRow parseRow(TextualTable table)
        {
            TextualRow row = new TextualRow(table);

            // | "val1" | 2 | "val3" |
            expectToken(TokenType.Pipe);
            while (!matchToken(TokenType.Dash))
            {
                if (matchToken(TokenType.Number))
                    row.SetValueOrdered(Convert.ToDouble(expectToken(TokenType.Number).Value));
                else
                    row.SetValueOrdered(expectToken(TokenType.String).Value);
                expectToken(TokenType.Pipe);
            }

            // -------------
            while (acceptToken(TokenType.Dash)) ;

            return row;
        }

        private bool matchToken(TokenType tokenType)
        {
            if (endOfStream)
                return false;
            return tokens[position].TokenType == tokenType;
        }
        private bool matchToken(TokenType tokenType, string value)
        {
            if (endOfStream)
                return false;
            return tokens[position].TokenType == tokenType && tokens[position].Value == value;
        }

        private bool acceptToken(TokenType tokenType)
        {
            bool res = matchToken(tokenType);
            if (res)
                position++;
            return res;
        }
        private bool acceptToken(TokenType tokenType, string value)
        {
            bool res = matchToken(tokenType, value);
            if (res)
                position++;
            return res;
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
