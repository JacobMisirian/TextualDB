using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Components.Operations.Exceptions
{
    public class UnknownWhereOperationException : OperationException
    {
        public override TextualOperation TextualOperation { get; }

        public new string Message { get { return message; } }

        private const string MESSAGE_FORMAT = "No such where operation \"{0}\"!";

        private string message;

        public UnknownWhereOperationException(TextualOperation operation, int op)
        {
            TextualOperation = operation;

            message = string.Format(MESSAGE_FORMAT, op);
        }
        public UnknownWhereOperationException(TextualOperation operation, string op)
        {
            TextualOperation = operation;

            message = string.Format(MESSAGE_FORMAT, op);
        }
    }
}
