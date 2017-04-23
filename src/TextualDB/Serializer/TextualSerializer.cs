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
            SerializeDatabase(stream, database);
        }

        public void SerializeDatabase(Stream stream, TextualDatabase database)
        {
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            foreach (var table in database.Tables.Values)
                serializeTable(table);
        }

        private void serializeTable(TextualTable table)
        {
            colLength = table.ColumnLength;

            writer.WriteLine(string.Format("{0}:", table.Name));

            foreach (var column in table.Columns)
                writer.Write(string.Format("{0} | ", column));
            writer.WriteLine();

            foreach (var row in table.Rows)
                serializeRow(row);
            writer.WriteLine("?");
        }

        private int colLength;
        private void serializeRow(TextualRow row)
        {
            int hyphenLength = 0;

            foreach (var val in row.Values.Values)
                hyphenLength += val.Value.Length + 4;

            hyphenLength += 2;

            hyphenLength = hyphenLength < colLength ? colLength : hyphenLength;

            for (int i = 0; i < hyphenLength; i++)
                writer.Write("-");

            writer.WriteLine();
            foreach (var col in row.Owner.Columns)
                writer.Write(string.Format("\"{0}\" | ", row.Values.ContainsKey(col) ? row.Values[col].Value : string.Empty));
            writer.WriteLine();

            for (int i = 0; i < hyphenLength; i++)
                writer.Write("-");
            writer.WriteLine();
        }
    }
}
