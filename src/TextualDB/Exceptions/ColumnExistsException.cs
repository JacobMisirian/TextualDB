using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    /// <summary>
    /// Exception to be thrown if an attempt is made to create a column that already exists in a given table
    /// </summary>
    public class ColumnExistsException : Exception
    {
        /// <summary>
        /// The string message showing the column name and table name
        /// </summary>
        public new string Message { get { return string.Format("Column {0} already exists in table {1}!", Column, Table.Name); } }
        /// <summary>
        /// The already existing column
        /// </summary>
        public string Column { get; private set; }
        /// <summary>
        /// The table where the column already exists
        /// </summary>
        public TextualTable Table { get; private set; }
        /// <summary>
        /// Constructs a new ColumnExistsException with the given column and table
        /// </summary>
        /// <param name="column">The already existing column</param>
        /// <param name="table">The table where the column already exists</param>
        public ColumnExistsException(string column, TextualTable table)
        {
            Column = column;
            Table = table;
        }
    }
}
