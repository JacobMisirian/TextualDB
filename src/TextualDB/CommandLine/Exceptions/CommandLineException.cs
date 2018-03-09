using System;

namespace TextualDB.CommandLine.Exceptions
{
    public abstract class CommandLineException : Exception
    {
        public abstract SourceLocation SourceLocation { get; }
    }
}
