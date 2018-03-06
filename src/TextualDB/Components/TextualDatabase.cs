using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.Components.Exceptions;
using TextualDB.Serialization;

namespace TextualDB.Components
{
    public class TextualDatabase : ISerializable
    {
        public string Name { get; private set; }
        public Dictionary<string, TextualTable> Tables { get; private set; }

        public TextualDatabase(string name)
        {
            Name = name;
            Tables = new Dictionary<string, TextualTable>();
        }

        public void AddTable(TextualTable table)
        {
            if (Tables.ContainsKey(table.Name))
                throw new TableAlreadyExistsException(this, table.Name);
            Tables.Add(table.Name, table);
        }

        public TextualTable GetTable(string name)
        {
            if (!Tables.ContainsKey(name))
                throw new TableNotFoundException(this, name);
            return Tables[name];
        }

        public void RemoveTable(string name)
        {
            if (!Tables.ContainsKey(name))
                throw new TableNotFoundException(this, name);
            Tables.Remove(name);
        }

        public void Serialize(StringBuilder sb)
        {
            foreach (var table in Tables.Values)
                table.Serialize(sb);
        }
    }
}
