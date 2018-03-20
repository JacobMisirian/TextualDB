using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using TextualDB.Components;
using TextualDB.Components.Operations;
using TextualDB.Deserialization.Exceptions;
using TextualDB.Deserialization.Lexer;
using TextualDB.Deserialization.Parser;

namespace TextualDB
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string source = File.ReadAllText(args[0]);
                var tokens = new Scanner(source).Scan();

                TextualDatabase db = new Parser(tokens).ParseDatabase(args[0]);
                
                TextualDB.CommandLine.UI.TestCommandPrompt(db);

                // SELECT first FROM people;
                TextualSelectOperation select = new TextualSelectOperation(db.GetTable("people"), "first");
                select.Execute();
                Console.WriteLine(select.Result.ToString());

                // SELECT age FROM people WHERE age > 18;
                select = new TextualSelectOperation(db.GetTable("people"), "first");
                select.FilterWhereExclusive(new TextualWhereCondition(WhereOperation.GreaterThan, "age", 18));
                select.Execute();
                Console.WriteLine(select.Result.ToString());

                // INSERT INTO people AT 1 VALUES last="Foreman", first="Ben";
                TextualInsertOperation insert = new TextualInsertOperation(db.GetTable("people"));
                insert.BuildRow("last", "Foreman").BuildRow("first", "Ben");
                insert.Execute(1);
                Console.WriteLine(insert.Result.ToString());

                // UPDATE people VALUES age=17 WHERE first="Ben";
                TextualUpdateOperation update = new TextualUpdateOperation(db.GetTable("people"), new string[] { "age" }, new object[] { 17 });
                update.FilterWhereExclusive(new TextualWhereCondition(WhereOperation.Equal, "first", "Ben"));
                update.Execute();
                Console.WriteLine(db.GetTable("people"));

                // DELETE COLUMN IN people AT 1
                TextualDeleteColumnOperation deleteColumn = new TextualDeleteColumnOperation(db.GetTable("people"));
                deleteColumn.Execute(1);
                Console.WriteLine(db.GetTable("people"));

                // DELETE ROW FROM people WHERE age < 19
                TextualDeleteRowOperation deleteRow = new TextualDeleteRowOperation(db.GetTable("people"));
                deleteRow.FilterWhereInclusive(new TextualWhereCondition(WhereOperation.LesserThan, "age", 19));
              //  deleteRow.Execute();
                Console.WriteLine(db.GetTable("people"));

                // RENAME COLUMN first TO firstName IN people
                TextualRenameColumnOperation renameColumn = new TextualRenameColumnOperation(db.GetTable("people"));
                renameColumn.Execute("first", "firstName");
                Console.WriteLine(db.GetTable("people"));

                // RENAME TABLE people TO snakes
                TextualRenameTableOperation renameTable = new TextualRenameTableOperation(db.GetTable("people"));
                renameTable.Execute("snakes");
                Console.WriteLine(db.GetTable("snakes"));

                // SELECT firstName FROM snakes AT 1, 0
                select = new TextualSelectOperation(db.GetTable("snakes"), "firstName");
                select.FilterWhereExclusive(1);
                select.FilterWhereInclusive(0);
                select.Execute();
                Console.WriteLine(select.Result);

                // SELECT * FROM people;
                Console.WriteLine(db.GetTable("snakes"));
                
            }
            catch (UnknownCharacterException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DeserializationException de)
            {
                Console.WriteLine(de.ToString());
            }
        }
    }
}
