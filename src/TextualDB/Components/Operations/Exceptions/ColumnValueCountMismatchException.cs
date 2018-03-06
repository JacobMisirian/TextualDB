namespace TextualDB.Components.Operations.Exceptions
{
    public class ColumnValueCountMismatchException : OperationException
    {
        public override TextualOperation TextualOperation { get; }

        public new string Message { get { return message; } }

        private const string MESSAGE_FORMAT = "The count of columns, \"{0}\", did not match the count of values, \"{1}\"!";

        private string message;

        public ColumnValueCountMismatchException(TextualOperation operation, int columnCount, int valueCount)
        {
            TextualOperation = operation;

            message = string.Format(MESSAGE_FORMAT, columnCount, valueCount);
        }
    }
}
