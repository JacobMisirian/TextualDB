namespace TextualDB.Components.Operations.Exceptions
{
    public class IncorrectValueFormatException : OperationException
    {
        public override TextualOperation TextualOperation { get; }

        public override string Message { get {  return message; } }
        public TextualTable TextualTable { get; private set; }

        private const string MESSAGE_FORMAT = "An attempted operation was made with an incorrect value format!";

        private string message;

        public IncorrectValueFormatException(TextualOperation operation, TextualTable table)
        {
            TextualOperation = operation;
            TextualTable = table;

            message = MESSAGE_FORMAT;
        }
    }
}
