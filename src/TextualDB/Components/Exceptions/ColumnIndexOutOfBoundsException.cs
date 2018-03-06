namespace TextualDB.Components.Exceptions
{
    public class ColumnIndexOutOfBoundsException : ComponentException
    {
        public override TextualDatabase TextualDatabase { get; }

        public new string Message { get { return message; } }
        public TextualTable TextualTable { get; private set; }

        private const string MESSAGE_FORMAT = "The table \"{0}\" does not contain a column at index \"{1}\"!";

        private string message;

        public ColumnIndexOutOfBoundsException(TextualDatabase database, TextualTable table, int index)
        {
            TextualDatabase = database;
            TextualTable = table;

            message = string.Format(MESSAGE_FORMAT, table.Name, index);
        }
    }
}
