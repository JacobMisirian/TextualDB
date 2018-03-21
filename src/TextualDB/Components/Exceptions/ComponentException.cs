using System;

namespace TextualDB.Components.Exceptions
{
    public abstract class ComponentException : Exception
    {
        public abstract new string Message { get; }
        public abstract TextualDatabase TextualDatabase { get; }
    }
}
