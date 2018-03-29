using System.Collections.Generic;
using System.IO;
using System.Text;

using TextualDB.Components.Exceptions;
using TextualDB.Deserialization;
using TextualDB.Deserialization.Exceptions;
using TextualDB.Deserialization.Lexer;
using TextualDB.Deserialization.Parser;
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

        public void Save(string path)
        {
            StringBuilder sb = new StringBuilder();
            Serialize(sb);
            File.WriteAllText(path, sb.ToString());
        }

        public void Serialize(StringBuilder sb)
        {
            foreach (var table in Tables.Values)
                table.Serialize(sb);
        }

        public static TextualDatabase Parse(string name, string contents)
        {
            var tokens = new Scanner(contents).Scan();
            return new Parser(tokens).ParseDatabase(name);
        }
    }
}
