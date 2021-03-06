﻿using TextualDB.CommandLine.Lexer;

namespace TextualDB.CommandLine.Exceptions
{
    public class ExpectedTokenException : CommandLineException
    {
        public override SourceLocation SourceLocation { get; }

        public override string Message { get { return message; } }

        private const string MESSAGE_FORMAT_PRIMARY = "Expected token of type \"{0}\" with value \"{1}\"!";
        private const string MESSAGE_FORMAT_SECONDARY = "Expected token of type \"{0}\"!";

        private string message;

        public ExpectedTokenException(SourceLocation location, TokenType tokenType)
        {
            SourceLocation = location;

            message = string.Format(MESSAGE_FORMAT_SECONDARY, tokenType.ToString());
        }
        public ExpectedTokenException(SourceLocation location, TokenType tokenType, string value)
        {
            SourceLocation = location;

            message = string.Format(MESSAGE_FORMAT_PRIMARY, tokenType.ToString(), value);
        }
    }
}
