using System;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    /// <summary>
    /// Exception thrown if an attempt is made to access, remove, or rename a non-existent table in a given database
    /// </summary>
    public class TableNotFoundException : Exception
    {
        /// <summary>
        /// The string message showing the table name and database file path
        /// </summary>
        public new string Message { get { return string.Format("Table {0} does not exist in database {1}!", TableName, Database.FilePath); } }
        /// <summary>
        /// The non-existent table
        /// </summary>
        public string TableName { get; private set; }
        /// <summary>
        /// The database where the table does not exist
        /// </summary>
        public TextualDatabase Database { get; private set; }
        /// <summary>
        /// Constructs a new TableNotFoundException with the given database and table
        /// </summary>
        /// <param name="database">The database where the table does not exist</param>
        /// <param name="table">The non-existent table</param>
        public TableNotFoundException(TextualDatabase database, string table)
        {
            Database = database;
            TableName = table;
        }
    }
}
