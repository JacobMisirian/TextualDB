using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TextualDB.Exceptions;
using TextualDB.Serializer;

namespace TextualDB.Components
{
    public class TextualTable
    {
        public string Name { get; set; }

        public List<string> Columns { get; private set; }
        public int ColumnLength { get { int total = 0; foreach (var col in Columns) total += col.Length + 2; return total + 2; } }
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

        public TextualTable AddColumn(string name, int pos = -1)
        {
            if (ContainsColumn(name))
                throw new ColumnExistsException(name, this);

            pos = pos == -1 ? Columns.Count : pos;

            Columns.Insert(pos, name);

            foreach (var row in Rows)
                row.AddValue(name, string.Empty);

            return this;
        }

        public void AddRow(TextualRow row, int pos = -1)
        {
            pos = pos == -1 ? Rows.Count : pos;
            Rows.Insert(pos, row);
        }
        public void AddRow(int pos = -1, params string[] values)
        {

            var row = new TextualRow(this);

            row.StartAutoValueAdding();

            foreach (var val in values)
                row.AddValue(val);

            AddRow(row, pos);
        }

        public TextualTable ChangeColumnName(string oldName, string newName)
        {
            if (!ContainsColumn(oldName))
                throw new ColumnNotFoundException(oldName, this);

            int index = Columns.IndexOf(oldName);
            Columns.Remove(oldName);
            Columns.Insert(index, newName);

            foreach (var row in Rows)
                row.ChangeColumnName(oldName, newName);

            return this;
        }

        public TextualTable ChangeTableName(string newName)
        {
            Name = newName;
            return this;
        }
        
        public bool ContainsColumn(string name)
        {
            return Columns.Contains(name);
        }

        public TextualRow GetRow(int pos)
        {
            if (pos < 0 || pos >= Rows.Count)
                throw new RowNotFoundException(this, pos);
            return Rows[pos];
        }

        public void RemoveColumn(string name)
        {
            if (!ContainsColumn(name))
                throw new ColumnNotFoundException(name, this);

            Columns.Remove(name);

            foreach (var row in Rows)
                row.RemoveValue(name);
        }

        public void RemoveRow(TextualRow row)
        {
            if (!Rows.Contains(row))
                throw new RowNotFoundException(this, row);
            Rows.Remove(row);
        }

        public TextualTable Select(params string[] columns)
        {
            if (columns.Length == 0)
                return this;

            TextualTable ret = new TextualTable(Name, columns);

            foreach (var row in Rows)
            {
                var row_ = new TextualRow(ret);
                foreach (string column in columns)
                {
                    if (!row.Values.ContainsKey(column))
                        throw new ColumnNotFoundException(column, this);
                    row_.AddValue(column, row.Values[column].Value);
                }
                ret.AddRow(row_);
            }

            return ret;
        }

        public override string ToString()
        {
            MemoryStream mstream = new MemoryStream();

            new TextualSerializer().SerializeDatabase(mstream, new TextualDatabase(string.Empty, new Dictionary<string, TextualTable> { { Name, this } }));

            return ASCIIEncoding.ASCII.GetString(mstream.ToArray());
        }
    }
}