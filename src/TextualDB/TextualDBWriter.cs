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

        public void Write(TextualDBDatabase database, string filePath)
        {
            this.database = database;
            writer = new StreamWriter(filePath);

            foreach (var table in database.Tables.Values)
                writer.Write(string.Format("{0}\n", table.ToString()));
            writer.Flush();
            writer.Close();
        }
    }
}
