using System;

namespace TextualDB.Components.Exceptions
{
    public abstract class ComponentException : Exception
    {
        public abstract TextualDatabase TextualDatabase { get; }
    }
}
