using System;

namespace TextualDB.Components.Operations.Exceptions
{
    public abstract class OperationException : Exception
    {
        public abstract TextualOperation TextualOperation { get; }
    }
}
