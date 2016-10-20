using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Components;

namespace TextualDB
{
    public class TextualDBReader
    {
        private StreamReader reader;

        public TextualDBReader(string filePath)
        {
            reader = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public TextualDBDatabase Read()
        {
            TextualDBDatabase database = new TextualDBDatabase();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
                database.Add(readTable());

            reader.Close();
            return database;
        }

        private TextualDBTable readTable()
        {
            burnWhiteSpace();
            TextualDBTable table = new TextualDBTable(readUntil(':'));
            reader.Read();
            burnWhiteSpace();
            readColumnDeclarations(table);

            while ((char)reader.Peek() != '?')
            {
                table.Rows.Add(readRow(table));
                burnWhiteSpace();
            }

            return table;
        }

        private void readColumnDeclarations(TextualDBTable table)
        {
            while ((char)reader.Peek() != '\n')
            {
                burnWhiteSpace();
                table.Columns.Add(readUntil('|').Trim());
                if (reader.Peek() == '|')
                    reader.Read();
            }
            burnWhiteSpace();
        }

        private TextualDBRow readRow(TextualDBTable table)
        {
            TextualDBRow row = new TextualDBRow();
            while ((char)reader.Peek() != '\n')
            {
                row.Add(readUntil('|').Trim());
                if (reader.Peek() == '|')
                    reader.Read();
            }
            burnWhiteSpace();
            return row;
        }

        private string readUntil(char c)
        {
            StringBuilder sb = new StringBuilder();
            while ((char)reader.Peek() != c && (char)reader.Peek() != '\n')
                sb.Append((char)reader.Read());
            return sb.ToString();
        }
        
        private void burnWhiteSpace()
        {
            while (char.IsWhiteSpace((char)reader.Peek()))
                reader.Read();
        }
    }
}
