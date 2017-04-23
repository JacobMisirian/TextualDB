using System;

using TextualDB.Deserializer;

namespace TextualDB.Exceptions
{
    class CommandLineVisitorException : Exception
    {
        public new string Message { get; private set; }
        public SourceLocation SourceLocation { get; private set; }

        public CommandLineVisitorException(SourceLocation location, string msgf, params object[] args)
        {
            Message = string.Format(msgf, args);
            SourceLocation = location;
        }
    }
}
