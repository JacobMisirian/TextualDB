using System;

namespace TextualDB.Components.Operations.Exceptions
{
    public abstract class OperationException : Exception
    {
        public abstract new string Message { get; }
        public abstract TextualOperation TextualOperation { get; }
    }
}
