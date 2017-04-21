using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using TextualDB.Components;
using TextualDB.Exceptions;

namespace TextualDB.Serializer
{
    public class TextualSerializer
    {
        private StreamWriter writer;

        public void SerializeDatabase(string path, TextualDatabase database)
        {
            var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            foreach (var table in database.Tables.Values)
                serializeTable(table);
        }

        private void serializeTable(TextualTable table)
        {
            writer.WriteLine(string.Format("{0}:", table.Name));

            foreach (var column in table.Columns)
                writer.Write(string.Format("{0} | ", column));
            writer.WriteLine();

            foreach (var row in table.Rows)
                serializeRow(row);
        }

        private void serializeRow(TextualRow row)
        {
            int colLength = row.Owner.ColumnLength + (row.Owner.Columns.Count * 2);

            for (int i = 0; i < colLength; i++)
                writer.Write("-");

            writer.WriteLine();
            foreach (var col in row.Owner.Columns)
                writer.Write(string.Format("\"{0}\" | ", row.Values[col].Value));
            writer.WriteLine();

            for (int i = 0; i < colLength; i++)
                writer.Write("-");
            writer.WriteLine();
        }
    }
}
