using System;

namespace TextualDB.CommandLine.Exceptions
{
    public abstract class CommandLineException : Exception
    {
        public abstract new string Message { get; }
        public abstract SourceLocation SourceLocation { get; }
    }
}
