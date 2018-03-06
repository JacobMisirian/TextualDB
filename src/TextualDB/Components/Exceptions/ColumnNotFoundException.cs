using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Components.Exceptions
{
    public class ColumnNotFoundException : ComponentException
    {
        public override TextualDatabase TextualDatabase { get; }

        public new string Message { get { return message; } }
        public TextualTable TextualTable { get; private set; }

        private const string MESSAGE_FORMAT = "The table \"{0}\" does not contain a column named \"{1}\"!";

        private string message;

        public ColumnNotFoundException(TextualDatabase database, TextualTable table, string column)
        {
            TextualDatabase = database;
            TextualTable = table;

            message = string.Format(MESSAGE_FORMAT, table.Name, column);
        }
    }
}
