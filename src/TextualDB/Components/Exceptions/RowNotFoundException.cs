using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Components.Exceptions
{
    public class RowNotFoundException : ComponentException
    {
        public override TextualDatabase TextualDatabase { get; }

        public new string Message { get { return message; } }
        public TextualTable TextualTable { get; private set; }
        public TextualRow TextualRow { get; private set; }

        private const string MESSAGE_FORMAT = "The table \"{0}\" does not contain the requested row!";

        private string message;

        public RowNotFoundException(TextualDatabase database, TextualTable table, TextualRow row)
        {
            TextualDatabase = database;

            TextualTable = table;
            TextualRow = row;

            message = string.Format(MESSAGE_FORMAT, table.Name);
        }
    }
}
