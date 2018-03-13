using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.Components.Exceptions;
using TextualDB.Serialization;

namespace TextualDB.Components
{
    public class TextualRow : ISerializable
    {
        public TextualTable ParentTable { get; private set; }
        public Dictionary<string, object> Values { get; private set; }

        public TextualRow(TextualTable parentTable)
        {
            ParentTable = parentTable;
            Values = new Dictionary<string, object>();

            ValidateWithParent();
        }

        public TextualRow(TextualRow copy, TextualTable newParent = null)
        {
            ParentTable = newParent ?? copy.ParentTable;
            Values = new Dictionary<string, object>(copy.Values);
        }

        public int CalculateLineLength()
        {
            int count = 4;
            foreach (var entry in Values.Values)
                count += entry.ToString().Length;
            count += (Values.Count + 1);
            count += (Values.Count * 2);

            return count;
        }

        public object GetValue(string column)
        {
            if (!Values.ContainsKey(column))
                throw new ColumnNotFoundException(ParentTable.ParentDatabase, ParentTable, column);
            return Values[column];
        }

        public void RenameColumn(string oldName, string newName)
        {
            if (!Values.ContainsKey(oldName))
                throw new ColumnNotFoundException(ParentTable.ParentDatabase, ParentTable, oldName);
            if (Values.ContainsKey(newName))
                throw new ColumnAlreadyExistsException(ParentTable.ParentDatabase, ParentTable, newName);

            var tmp = Values[oldName];
            Values.Remove(oldName);
            Values.Add(newName, tmp);
        }

        public void SetValue(string column, object value)
        {
            if (!Values.ContainsKey(column))
                throw new ColumnNotFoundException(ParentTable.ParentDatabase, ParentTable, column);
            Values[column] = value;
        }

        private int valuePos = 0;
        public void SetValueOrdered(object value)
        {
            if (valuePos < ParentTable.Columns.Count)
                Values[ParentTable.Columns[valuePos++]] = value;
        }

        public void ValidateWithParent()
        {
            foreach (var column in ParentTable.Columns)
                if (!Values.ContainsKey(column))
                    Values.Add(column, string.Empty);

            string[] columns = Values.Keys.ToArray();
            for (int i = 0; i < columns.Length; i++)
                if (!ParentTable.Columns.Contains(columns[i]))
                    Values.Remove(columns[i]);
        }

        public void Serialize(StringBuilder sb)
        {
            // | 
            sb.Append("|");
            // "val1" | 2 | "val3" |
            foreach (var column in ParentTable.Columns)
            {
                var val = Values[column];
                if (val is double)
                    sb.AppendFormat(" {0} |", val.ToString());
                else
                    sb.AppendFormat(" \"{0}\" |", val.ToString());
            }
            sb.Append('\n');
        }
    }
}
