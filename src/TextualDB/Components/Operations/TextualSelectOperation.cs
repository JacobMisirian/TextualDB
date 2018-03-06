using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Components.Operations
{
    public class TextualSelectOperation : TextualOperation
    {
        public override TextualTable Result { get; }
        
        private TextualTable table;
        private string[] columns;

        private List<TextualRow> mirroredSourceRows;

        public TextualSelectOperation(TextualDatabase database, TextualTable table, params string[] columns)
        {
            Result = new TextualTable(database, table.Name);
            
            this.table = table;
            if (columns.Length == 0)
                this.columns = table.Columns.ToArray();
            else
                this.columns = columns;

            mirroredSourceRows = new List<TextualRow>();
        }

        public void Execute()
        {
            foreach (var column in columns)
                Result.AddColumn(column);

            foreach (var srcRow in table.Rows)
            {
                var destRow = new TextualRow(Result);
                foreach (var column in columns)
                    destRow.SetValue(column, srcRow.Values[column]);
                Result.AddRow(destRow);
                mirroredSourceRows.Add(srcRow);
            }
        }

        public void FilterWhereExclusive(TextualWhereCondition condition)
        {
            for (int i = 0; i < mirroredSourceRows.Count; i++)
            {
                var srcRow = mirroredSourceRows[i];
                if (!condition.Check(this, srcRow))
                {
                    mirroredSourceRows.Remove(srcRow);
                    Result.Rows.RemoveAt(i);
                }
            }
        }

        public void FilterWhereInclusive(TextualWhereCondition condition)
        {
            foreach (var srcRow in table.Rows)
            {
                if (condition.Check(this, srcRow))
                {
                    var destRow = new TextualRow(Result);
                    foreach (var column in columns)
                        destRow.SetValue(column, srcRow.Values[column]);
                }
            }
        }
    }
}
