namespace TextualDB.Components.Exceptions
{
    public class TableNotFoundException : ComponentException
    {
        public override TextualDatabase TextualDatabase { get; }

        public override string Message { get { return message; } }

        private const string MESSAGE_FORMAT = "The database \"{0}\" does not contain a table named \"{1}\"!";

        private string message;

        public TableNotFoundException(TextualDatabase database, string tableName)
        {
            TextualDatabase = database;

            message = string.Format(MESSAGE_FORMAT, database.Name, tableName);
        }
    }
}
