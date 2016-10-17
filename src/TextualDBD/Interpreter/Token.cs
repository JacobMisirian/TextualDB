using System;

namespace TextualDBD.Interpreter
{
    public class Token
    {
        public TokenType TokenType { get; private set; }
        public string Value { get; private set; }

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public static Token Create(TokenType tokenType, string value)
        {
            return new Token(tokenType, value);
        }
    }
}

