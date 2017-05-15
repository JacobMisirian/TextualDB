using System;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    /// <summary>
    /// Exception to be thrown if an attempt is made to access, modify, or remove a non-existent row in a given table
    /// </summary>
    public class RowNotFoundException : Exception
    {
        /// <summary>
        /// The string message showing the table name and optionally the row index
        /// </summary>
        public new string Message {  get { if (Row != null) return string.Format("No such row in table {0}!", Table.Name); else return string.Format("No such row '{0}' in table {1}!", Position, Table.Name); } }
        /// <summary>
        /// The non-existent row
        /// </summary>
        public TextualRow Row { get; private set; }
        /// <summary>
        /// The non-existent row index
        /// </summary>
        public int Position { get; private set; }
        /// <summary>
        /// The table the row does not exist in
        /// </summary>
        public TextualTable Table { get; private set; }
        /// <summary>
        /// Constructs a new RowNotFoundException with the given table and row
        /// </summary>
        /// <param name="table">The table where the row does not exist</param>
        /// <param name="row">The non-existent row</param>
        public RowNotFoundException(TextualTable table, TextualRow row)
        {
            Table = table;
            Row = row;
        }
        /// <summary>
        /// Constructs a new RowNotFoundException with the given table and row index
        /// </summary>
        /// <param name="table">The table where the position does not exist</param>
        /// <param name="position">The non-existent position</param>
        public RowNotFoundException(TextualTable table, int position)
        {
            Table = table;
            Position = position;
        }
    }
}
