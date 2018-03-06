using System;

namespace TextualDB.Deserialization.Exceptions
{
    public abstract class DeserializerException : Exception
    {
        public abstract SourceLocation SourceLocation { get; }
    }
}
