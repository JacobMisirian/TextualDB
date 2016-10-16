using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Components;

namespace TextualDB.Exceptions
{
    public class TableNotFoundException : Exception
    {
        public new string Message { get { return string.Format("Could not find table {0} in database!", Table); } }
        public string Table { get; private set; }
        public TextualDBDatabase Database { get; private set; }
        
        public TableNotFoundException(string table, TextualDBDatabase database)
        {
            Table = table;
            Database = database;
        }
    }
}
