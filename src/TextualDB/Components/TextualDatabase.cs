using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TextualDB.Deserializer;
using TextualDB.Deserializer.Lexer;
using TextualDB.Exceptions;

namespace TextualDB.Components
{
    public class TextualDatabase
    {
        public static TextualDatabase FromFile(string filePath)
        {
            return new TextualParser(new Scanner().Scan(filePath, File.ReadAllText(filePath))).ParseDatabase(filePath);
        }
        public string FilePath { get; private set; }
        
        public Dictionary<string, TextualTable> Tables { get; private set; }

        public TextualDatabase(string filePath)
        {
            FilePath = filePath;
            Tables = new Dictionary<string, TextualTable>();
        }
        public TextualDatabase(string filePath, Dictionary<string, TextualTable> tables)
        {
            FilePath = filePath;
            Tables = tables;
        }

        public TextualDatabase AddTable(TextualTable table)
        {
            if (ContainsTable(table.Name))
                throw new TableExistsException(table.Name, this);
            Tables.Add(table.Name, table);
            return this;
        }
        public TextualDatabase AddTable(string name)
        {
            return AddTable(new TextualTable(name));
        }
        public TextualDatabase AddTable(string name, params string[] columns)
        {
            return AddTable(new TextualTable(name, columns));
        }
        public TextualDatabase AddTable(string name, IEnumerable<string> columns, IEnumerable<TextualRow> rows)
        {
            return AddTable(new TextualTable(name, columns.ToList(), rows.ToList()));
        }

        public bool ContainsTable(string name)
        {
            return Tables.ContainsKey(name);
        }

        public TextualTable GetTable(string name)
        {
            if (!ContainsTable(name))
                throw new TableNotFoundException(this, name);
            return Tables[name];
        }

        public void RemoveTable(string name)
        {
            if (!ContainsTable(name))
                throw new TableNotFoundException(this, name);
            Tables.Remove(name);
        }
    }
}
