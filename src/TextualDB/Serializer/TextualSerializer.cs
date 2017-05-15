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
        public void SerializeDatabase(string path, TextualDatabase database)
        {
            File.Delete(path);
            var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            SerializeDatabase(stream, database);
        }

        public void SerializeDatabase(Stream stream, TextualDatabase database)
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.AutoFlush = true;

                foreach (var table in database.Tables.Values)
                    serializeTable(writer, table);
            }
        }

        private void serializeTable(StreamWriter writer, TextualTable table)
        {
            colLength = table.ColumnLength;
            lastHyphenLength = colLength;

            writer.WriteLine(string.Format("{0}:", table.Name));

            foreach (var column in table.Columns)
                writer.Write(string.Format("{0} | ", column));
            writer.WriteLine();

            foreach (var row in table.Rows)
                serializeRow(writer, row);
          
            for (int i = 0; i < lastHyphenLength; i++)
                writer.Write("-");
            writer.WriteLine();

            writer.WriteLine("?");
        }

        private int colLength;
        private int lastHyphenLength;
        private void serializeRow(StreamWriter writer, TextualRow row)
        {
            int hyphenLength = 0;

            foreach (var val in row.Values.Values)
                hyphenLength += val.Length + 4;

            hyphenLength += 4;

            var temp = hyphenLength;
            hyphenLength = hyphenLength < lastHyphenLength ? lastHyphenLength : hyphenLength;
            lastHyphenLength = temp;

            for (int i = 0; i < hyphenLength; i++)
                writer.Write("-");
            writer.WriteLine();

            writer.Write("| ");
            foreach (var col in row.Owner.Columns)
                writer.Write(string.Format("\"{0}\" | ", row.Values.ContainsKey(col) ? row.Values[col] : string.Empty));
            writer.WriteLine();
        }
    }
}
