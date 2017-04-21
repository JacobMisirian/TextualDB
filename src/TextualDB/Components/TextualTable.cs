using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Components
{
    public class TextualTable
    {
		public string Name { get; private set; }

		public List<string> Columns { get; private set; }
		public int ColumnLength {  get { int total = 0; foreach (var col in Columns) total += col.Length; return total; } }
		public List<TextualRow> Rows { get; private set; }

		public TextualTable(string name, params string[] columns)
        {
            Name = name;
            Columns = columns.ToList();
            Rows = new List<TextualRow>();
        }

		public TextualTable(string name, List<string> columns, List<TextualRow> rows)
        {
            Name = name;
            Columns = columns;
            Rows = rows;
        }
    }
}