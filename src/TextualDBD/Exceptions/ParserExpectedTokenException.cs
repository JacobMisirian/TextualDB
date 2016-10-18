using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDBD.Interpreter;

namespace TextualDBD.Exceptions
{
    public class ParserExpectedTokenException : Exception
    {
        public new string Message { get; private set; }

        public ParserExpectedTokenException(TokenType tokenType, Token got)
        {
            Message = string.Format("Expected token type {0}, instead got token type {1} with value {2}!", tokenType, got.TokenType, got.Value);
        }
        public ParserExpectedTokenException(TokenType tokenType, string value, Token got)
        {
            Message = string.Format("Expected token type {0} with value {1}, instead got token type {2} with value {3}!", tokenType, value, got.TokenType, got.Value);
        }
    }
}
