using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Exceptions;

namespace TextualDB.Components
{
    public class TextualDBDatabase
    {
        public Dictionary<string, TextualDBTable> Tables { get; private set; }

        public TextualDBDatabase()
        {
            Tables = new Dictionary<string, TextualDBTable>();
        }
        public TextualDBDatabase(Dictionary<string, TextualDBTable> tables)
        {
            Tables = tables;
        }

        public TextualDBDatabase Add(TextualDBTable table)
        {
            if (Tables.ContainsKey(table.Name))
                throw new TableAlreadyExistsException(table.Name);
            Tables.Add(table.Name, table);
            return this;
        }

        public TextualDBDatabase Drop(string table)
        {
            if (!Tables.ContainsKey(table))
                throw new TableNotFoundException(table, this);
            Tables.Remove(table);
            return this;
        }

        public TextualDBTable Select(string table)
        {
            if (!Tables.ContainsKey(table))
                throw new TableNotFoundException(table, this);
            return Tables[table];
        }

        public List<TextualDBRow> SelectFrom(string table, params int[] rows)
        {
            return Select(table).Select(rows);
        }

        public List<TextualDBRow> SelectFromWhere(string table, string column, string value)
        {
            return Select(table).SelectWhere(column, value);
        }

        public int ResolveColumnNumber(string table, string column)
        {
            return Select(table).ResolveColumnNumber(column);
        }
    }
}