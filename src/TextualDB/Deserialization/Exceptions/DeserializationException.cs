using System;

namespace TextualDB.Deserialization.Exceptions
{
    public abstract class DeserializationException : Exception
    {
        public abstract SourceLocation SourceLocation { get; }
    }
}
