using System;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    public class TableNotFoundException : Exception
    {
        public new string Message { get { return string.Format("Table {0} does not exist!", TableName); } }

        public TextualDatabase Database { get; private set; }
        public string TableName { get; private set; }

        public TableNotFoundException(TextualDatabase database, string table)
        {
            Database = database;
            TableName = table;
        }
    }
}
