using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDBD.Interpreter;

namespace TextualDBD.Exceptions
{
    public class ParserUnexpectedTokenException : Exception
    {
        public new string Message {  get { return string.Format("Unexpected token of type {0} and value {1}!", Token.TokenType, Token.Value); } }
        public Token Token { get; private set; }

        public ParserUnexpectedTokenException(Token token)
        {
            Token = token;
        }
    }
}
