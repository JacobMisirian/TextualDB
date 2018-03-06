namespace TextualDB.Components.Exceptions
{
    public class ColumnAlreadyExistsException : ComponentException
    {
        public override TextualDatabase TextualDatabase { get; }
        
        public new string Message { get { return message; } }
        public TextualTable TextualTable { get; private set; }

        private const string MESSAGE_FORMAT = "The table \"{0}\" already contains a column named \"{1}\"!";

        private string message;

        public ColumnAlreadyExistsException(TextualDatabase database, TextualTable table, string column)
        {
            TextualDatabase = database;
            TextualTable = table;

            message = string.Format(MESSAGE_FORMAT, table.Name, column);
        }
    }
}
