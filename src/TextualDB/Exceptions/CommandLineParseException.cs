using System;

using TextualDB.Deserializer;

namespace TextualDB.Exceptions
{
    public class CommandLineParseException : Exception
    {
        public new string Message { get; private set; }
        public SourceLocation SourceLocation { get; private set; }

        public CommandLineParseException(SourceLocation location, string msgf, params object[] args)
        {
            Message = string.Format(msgf, args);
            SourceLocation = location;
        }
    }
}
