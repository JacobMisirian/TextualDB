using System;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    class TableExistsException : Exception
    {
        public new string Message { get { return string.Format("Table {0} already exists in database {1}!", Table, Database.FilePath); } }

        public string Table { get; private set; }
        public TextualDatabase Database { get; private set; }

        public TableExistsException(string table, TextualDatabase database)
        {
            Table = table;
            Database = database;
        }
    }
}
