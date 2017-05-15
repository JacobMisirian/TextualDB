using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    /// <summary>
    /// Exception to be thrown if an attempt is made to access, remove, or rename a non-existent column in a given table
    /// </summary>
    public class ColumnNotFoundException : Exception
    {
        /// <summary>
        /// The string message showing the column and table name
        /// </summary>
        public new string Message { get { return string.Format("Column {0} does not exist in table {1}!", Column, Table.Name); } }
        /// <summary>
        /// The non-existent column
        /// </summary>
        public string Column { get; private set; }
        /// <summary>
        /// The table the column does not exist in
        /// </summary>
        public TextualTable Table { get; private set; }
        /// <summary>
        /// Constructs a new ColumnNotFoundException with the given column and table
        /// </summary>
        /// <param name="column">The non-existent column</param>
        /// <param name="table">The table where the column does not exist</param>
        public ColumnNotFoundException(string column, TextualTable table)
        {
            Column = column;
            Table = table;
        }
    }
}
