using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.CommandLine.Exceptions
{
    public class UnknownCharacterException : CommandLineException
    {
        public override SourceLocation SourceLocation { get; }

        public new string Message { get { return message; } }

        private const string MESSAGE_FORMAT = "Unknown character \"{0}\"!";
        private string message;

        public UnknownCharacterException(SourceLocation location, char c)
        {
            SourceLocation = location;

            message = string.Format(MESSAGE_FORMAT, c.ToString());
        }
    }
}
