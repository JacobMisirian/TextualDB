using System.Collections.Generic;
using System.Text;

using TextualDB.Components.Exceptions;
using TextualDB.Serialization;

namespace TextualDB.Components
{
    public class TextualTable : ISerializable
    {
        public string Name { get; private set; }
        public TextualDatabase ParentDatabase { get; private set; }

        public List<string> Columns { get; private set; }
        public List<TextualRow> Rows { get; private set; }

        public TextualTable(TextualDatabase parentDatabase, string name)
        {
            Name = name;
            ParentDatabase = parentDatabase;

            Columns = new List<string>();
            Rows = new List<TextualRow>();
        }

        public void AddColumn(string name, int index = -1)
        {
            if (Columns.Contains(name))
                throw new ColumnAlreadyExistsException(ParentDatabase, this, name);
            if (index == -1)
                Columns.Add(name);
            else if (index < 0 || index > Columns.Count)
                throw new ColumnIndexOutOfBoundsException(ParentDatabase, this, index);
            else
                Columns.Insert(index, name);

            foreach (var row in Rows)
                row.ValidateWithParent();
        }

        public void AddRow(TextualRow row)
        {
            Rows.Add(row);
        }
        public void AddRow(TextualRow row, int index)
        {
            if (index < 0 || index > Rows.Count)
                throw new RowIndexOutOfBoundsException(ParentDatabase, this, index);
            Rows.Insert(index, row);
        }

        public TextualRow GetRow(int index)
        {
            if (index < 0 || index >= Rows.Count)
                throw new RowIndexOutOfBoundsException(ParentDatabase, this, index);
            return Rows[index];
        }

        public void RenameColumn(string oldName, string newName)
        {
            if (!Columns.Contains(oldName))
                throw new ColumnNotFoundException(ParentDatabase, this, oldName);
            if (Columns.Contains(newName))
                throw new ColumnAlreadyExistsException(ParentDatabase, this, newName);

            Columns[Columns.IndexOf(oldName)] = newName;

            foreach (var row in Rows)
                row.RenameColumn(oldName, newName);
        }

        public void RemoveColumn(int index)
        {
            if (index < 0 || index >= Columns.Count)
                throw new ColumnIndexOutOfBoundsException(ParentDatabase, this, index);
            RemoveColumn(Columns[index]);
        }
        public void RemoveColumn(string name)
        {
            if (!Columns.Contains(name))
                throw new ColumnNotFoundException(ParentDatabase, this, name);
            Columns.Remove(name);
            foreach (var row in Rows)
                row.ValidateWithParent();
        }

        public void RemoveRow(int index)
        {
            if (index < 0 || index >= Rows.Count)
                throw new RowIndexOutOfBoundsException(ParentDatabase, this, index);
            RemoveRow(GetRow(index));
        }
        public void RemoveRow(TextualRow row)
        {
            if (!Rows.Contains(row))
                throw new RowNotFoundException(ParentDatabase, this, row);
            Rows.Remove(row);
        }

        public void Serialize(StringBuilder sb)
        {
            // name:
            sb.AppendFormat("{0}:\n", Name);

            // | column1 | column2 | column3 |
            sb.Append("|");
            foreach (var column in Columns)
                sb.AppendFormat(" {0} |", column);
            sb.Append('\n');

            if (Rows.Count > 0)
            {
                // --------------------------
                int firstLineLength = Rows[0].CalculateLineLength();
                for (int i = 0; i < firstLineLength; i++)
                    sb.Append('-');
                sb.AppendLine();

                foreach (var row in Rows)
                {
                    // | "val1" | 2 | "val3" |
                    row.Serialize(sb);
                    int rowLength = row.CalculateLineLength();
                    // ---------------------------
                    for (int i = 0; i < rowLength; i++)
                        sb.Append('-');
                    sb.Append('\n');
                }
            }
            // ?
            sb.Append("?\n");
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Serialize(sb);
            return sb.ToString();
        }
    }
}
