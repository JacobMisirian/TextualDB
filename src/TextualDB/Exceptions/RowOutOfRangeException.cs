using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    public class RowOutOfRangeException : Exception
    {
        public new string Message {  get { return string.Format("Row number {0} was outside the bounds of table {0}!", RowIndex, Table.Name); } }
        public TextualDBTable Table { get; private set; }
        public int RowIndex { get; private set; }

        public RowOutOfRangeException(int rowIndex, TextualDBTable table)
        {
            RowIndex = rowIndex;
            Table = table;
        }
    }
}
