using System;
using System.IO;

using TextualDB.Components;
using TextualDB.Components.Exceptions;

namespace TextualDB.CommandLine
{
    public class UIState
    {
        public TextualDatabase Database { get; set; }
        public string DatabaseFilePath {  get { return Database != null ? Database.Name : " "; } }

        public bool PersistChanges { get; set; }

        public string Prompt { get { return string.Format("|{0}|{1}|>", DatabaseFilePath, PersistChanges); } }

        public UIState()
        {
            PersistChanges = true;
        }

        public bool OpenDatabase(string file)
        {
            try
            {
                Database = TextualDatabase.Parse(file, File.ReadAllText(file));
            }
            catch (ComponentException ce)
            {
                throw ce;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void SaveDatabase()
        {
            SaveDatabase(Database.Name);
        }
        public void SaveDatabase(string path)
        {
            Database.Save(path);
        }
    }
}
