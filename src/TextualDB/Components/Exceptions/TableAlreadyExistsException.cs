namespace TextualDB.Components.Exceptions
{
    public class TableAlreadyExistsException : ComponentException
    {
        public override TextualDatabase TextualDatabase { get; }

        public new string Message { get { return message; } }

        private const string MESSAGE_FORMAT = "The database \"{0}\" already contains a table named \"{1}\"!";

        private string message;

        public TableAlreadyExistsException(TextualDatabase database, string tableName)
        {
            TextualDatabase = database;

            message = string.Format(MESSAGE_FORMAT, tableName);
        }
    }
}
