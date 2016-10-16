using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Components;

namespace TextualDB
{
    public class TextualDBWriter
    {
        private StreamWriter writer;
        private TextualDBDatabase database;

        public TextualDBWriter(TextualDBDatabase database)
        {
            this.database = database;
        }

        public void Write(string filePath)
        {
            writer = new StreamWriter(filePath);

            foreach (var table in database.Tables.Values)
                writer.WriteLine(table.ToString());
            writer.Flush();
            writer.Close();
        }
    }
}
