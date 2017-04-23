using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    public class ColumnExistsException : Exception
    {
        public new string Message { get { return string.Format("Column {0} does not exist in table {1}!", Column, Table.Name); } }

        public string Column { get; private set; }
        public TextualTable Table { get; private set; }

        public ColumnExistsException(string column, TextualTable table)
        {
            Column = column;
            Table = table;
        }
    }
}
