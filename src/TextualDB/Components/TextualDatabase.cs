﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Exceptions;

namespace TextualDB.Components
{
    public class TextualDatabase
    {
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
            Tables.Add(table.Name, table);
            return this;
        }
        public TextualDatabase AddTable(string name)
        {
            Tables.Add(name, new TextualTable(name));
            return this;
        }
        public TextualDatabase AddTable(string name, params string[] rows)
        {
            Tables.Add(name, new TextualTable(name, rows));
            return this;
        }
        public TextualDatabase AddTable(string name, List<string> columns, List<TextualRow> rows)
        {
            Tables.Add(name, new TextualTable(name, columns, rows));
            return this;
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
