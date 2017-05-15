using System;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    /// <summary>
    /// Exception to be thrown if an attempt is made to create a table that already exists in a given database
    /// </summary>
    public class TableExistsException : Exception
    {
        /// <summary>
        /// The string message showing the table name and database file path
        /// </summary>
        public new string Message { get { return string.Format("Table {0} already exists in database {1}!", Table, Database.FilePath); } }
        /// <summary>
        /// The already existing table
        /// </summary>
        public string Table { get; private set; }
        /// <summary>
        /// The database where the table already exists
        /// </summary>
        public TextualDatabase Database { get; private set; }
        /// <summary>
        /// Constructs a new TableExistsException with the given table and database
        /// </summary>
        /// <param name="table">The already existing table</param>
        /// <param name="database">The database where the table already exists</param>
        public TableExistsException(string table, TextualDatabase database)
        {
            Table = table;
            Database = database;
        }
    }
}
