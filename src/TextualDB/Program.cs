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

                // SELECT first FROM people;
                TextualSelectOperation select = new TextualSelectOperation(db, db.GetTable("people"), "first");
                select.Execute();
                Console.WriteLine(select.Result.ToString());

                // SELECT age FROM people WHERE age > 18;
                select.FilterWhereExclusive(new TextualWhereCondition(WhereOperation.GreaterThan, "age", 18));
                Console.WriteLine(select.Result.ToString());

                // INSERT INTO people AT 1 VALUES last="Foreman", first="Ben";
                TextualInsertOperation insert = new TextualInsertOperation(db.GetTable("people"), 1);
                insert.BuildRow("last", "Foreman").BuildRow("first", "Ben");
                insert.Execute();
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
                deleteRow.Execute();
                Console.WriteLine(db.GetTable("people"));

                // RENAME COLUMN first TO firstName IN people
                TextualRenameColumnOperation renameColumn = new TextualRenameColumnOperation(db.GetTable("people"));
                renameColumn.Execute("first", "firstName");
                Console.WriteLine(db.GetTable("people"));

                // SELECT * FROM people;
                Console.WriteLine(db.GetTable("people"));
                
            }
            catch (UnknownCharacterException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DeserializerException de)
            {
                Console.WriteLine(de.ToString());
            }
        }
    }
}
