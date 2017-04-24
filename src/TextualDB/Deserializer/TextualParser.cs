using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.Components;
using TextualDB.Deserializer.Lexer;
using TextualDB.Exceptions;

namespace TextualDB.Deserializer
{
    public class TextualParser
    {
        private int position;
        private List<Token> tokens;

        private Token currentToken { get { return tokens[position]; } }
        private bool eof { get { return position >= tokens.Count; } }

        public TextualParser(List<Token> tokens)
        {
            position = 0;
            this.tokens = tokens;
        }

        public TextualDatabase ParseDatabase(string filePath)
        {
            TextualDatabase database = new TextualDatabase(filePath);

            while (!eof)
                database.AddTable(parseTable());

            return database;
        }

        private TextualTable parseTable()
        {
            string name = expectToken(TokenType.Identifier).Value;
            acceptToken(TokenType.Colon);
            acceptToken(TokenType.Pipe);

            List<string> columns = new List<string>();
            while (matchToken(TokenType.Identifier))
            {
                columns.Add(expectToken(TokenType.Identifier).Value);
                acceptToken(TokenType.Pipe);
            }
            while (acceptToken(TokenType.Hyphen)) ;

            List<TextualRow> rows = new List<TextualRow>();
            TextualTable table = new TextualTable(name, columns, rows);

            while (!acceptToken(TokenType.QuestionMark))
                rows.Add(parseRow(table));
            return table;
        }

        private TextualRow parseRow(TextualTable table)
        {
            acceptToken(TokenType.Pipe);
            while (acceptToken(TokenType.Hyphen)) ;
            acceptToken(TokenType.Pipe);

            TextualRow row = new TextualRow(table);
            row.StartAutoValueAdding();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                row.AddValue(expectToken(TokenType.String).Value);
                acceptToken(TokenType.Pipe);
            }
            while (acceptToken(TokenType.Hyphen)) ;
            row.EndAutoValueAdding();
            return row;
        }

        private bool matchToken(TokenType tokenType)
        {
            return currentToken.TokenType == tokenType && !eof;
        }
        private bool matchToken(TokenType tokenType, string val)
        {
            return currentToken.TokenType == tokenType && currentToken.Value == val && !eof;
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
                throw new DeserializerException(currentToken.SourceLocation, "Expected token of type {0}, got {1}!", tokenType, currentToken.TokenType);
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
