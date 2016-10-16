using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    public class ColumnOutOfRangeException : Exception
    {
        public new string Message {  get { return string.Format("Column number {0} was outside the bounds of table {1}!", ColumnIndex, Table.Name); } }
        public int ColumnIndex { get; private set; }
        public TextualDBTable Table { get; private set; }

        public ColumnOutOfRangeException(int columnIndex, TextualDBTable table)
        {
            ColumnIndex = columnIndex;
            Table = table;
        }
    }
}
