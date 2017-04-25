using System;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    public class RowNotFoundException : Exception
    {
        public new string Message {  get { if (Row != null) return string.Format("No such row in table {0}!", Table.Name); else return string.Format("No such row '{0}' in table {1}!", Position, Table.Name); } }

        public TextualTable Table { get; private set; }
        public TextualRow Row { get; private set; }
        public int Position { get; private set; }

        public RowNotFoundException(TextualTable table, TextualRow row)
        {
            Table = table;
            Row = row;
        }

        public RowNotFoundException(TextualTable table, int position)
        {
            Table = table;
            Position = position;
        }
    }
}
