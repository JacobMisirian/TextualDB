using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    public class ColumnAlreadyExistsException : Exception
    {
        public new string Message {  get { return string.Format("There is already a column named {0} in table {1}!", ColumnName, Table.Name); } }
        public string ColumnName { get; private set; }
        public TextualDBTable Table { get; private set; }

        public ColumnAlreadyExistsException(string columnName, TextualDBTable table)
        {
            ColumnName = columnName;
            Table = table;
        }
    }
}
