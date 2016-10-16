using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    public class TableAlreadyExistsException : Exception
    {
        public new string Message {  get { return string.Format("There is already a table named {0} in the database!", TableName); } }
        public string TableName { get; private set; }

        public TableAlreadyExistsException(string tableName)
        {
            TableName = tableName;
        }
    }
}
