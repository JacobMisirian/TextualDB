using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Exceptions;

namespace TextualDB.Components
{
    public class TextualDBTable
    {
        public string Name { get;  set; }
        public List<string> Columns { get; private set; }
        public List<TextualDBRow> Rows { get; private set; }

        public TextualDBTable(string name)
        {
            Name = name;
            Columns = new List<string>();
            Rows = new List<TextualDBRow>();
        }
        public TextualDBTable(string name, List<string> columns)
        {
            Name = name;
            Columns = columns;
            Rows = new List<TextualDBRow>();
        }
        public TextualDBTable(string name, List<string> columns, List<TextualDBRow> rows)
        {
            Name = name;
            Columns = columns;
            Rows = rows;
        }

        public TextualDBTable AddColumn(string name)
        {
            if (Columns.Contains(name))
                throw new ColumnAlreadyExistsException(name, this);
            Columns.Add(name);
            return this;
        }
        public TextualDBTable AddColumn(string name, int position)
        {
            if (position >= Columns.Count || position < 0)
                throw new ColumnOutOfRangeException(position, this);
            foreach (var row in Rows)
                row.Data.Insert(position, "");
            Columns.Insert(position, name);
            return this;
        }

        public TextualDBTable RemoveColumn(string name)
        {
            return RemoveColumn(Columns.IndexOf(name));
        }
        public TextualDBTable RemoveColumn(int index)
        {
            if (index >= Columns.Count || index < 0)
                throw new ColumnOutOfRangeException(index, this);
            foreach (var row in Rows)
                row.Data.RemoveAt(index);
            Columns.RemoveAt(index);
            return this;
        }

        public List<TextualDBRow> Select(params int[] rows)
        {
            List<TextualDBRow> result = new List<TextualDBRow>();
            foreach (int i in rows)
            {
                if (i == -1)
                {
                    result = Rows;
                    return result;
                }
                if (Rows.Count >= i || i < 0)
                    throw new RowOutOfRangeException(i, this);
                result.Add(Rows[i]);
            }
                
            return result;
        }

        public List<TextualDBRow> SelectWhere(string column, string value)
        {
            return SelectWhere(Columns.IndexOf(column), value);
        }
        public List<TextualDBRow> SelectWhere(int column, string value)
        {
            if (column >= Columns.Count || column < 0)
                throw new ColumnOutOfRangeException(column, this);
            List<TextualDBRow> result = new List<TextualDBRow>();
            foreach (var row in Rows)
                if (row.Data[column] == value)
                    result.Add(row);
            return result;
        }

        public int ResolveColumnNumber(string column)
        {
            if (!Columns.Contains(column))
                throw new ColumnOutOfRangeException(-1, this);
            return Columns.IndexOf(column);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}:\n", Name);
            foreach (var column in Columns)
                sb.AppendFormat("{0} | ", column);
            sb.AppendLine();
            foreach (var row in Rows)
                sb.AppendLine(row.ToString());
            sb.Append("?");
            return sb.ToString();
        }
    }
}