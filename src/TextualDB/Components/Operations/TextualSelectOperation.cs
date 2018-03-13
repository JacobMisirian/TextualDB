using System.Collections.Generic;

namespace TextualDB.Components.Operations
{
    public class TextualSelectOperation : TextualOperation
    {
        public override TextualTable Result { get; }

        private TextualTable table;

        public TextualSelectOperation(TextualTable table, params string[] columns)
        {
            Result = new TextualTable(table.ParentDatabase, table.Name);
            this.table = table;

            if (columns.Length == 0)
                columns = table.Columns.ToArray();

            foreach (var column in columns)
                Result.AddColumn(column);
            foreach (var srcRow in table.Rows)
                Result.AddRow(srcRow);
        }

        public void Execute()
        {
            for (int i = 0; i < Result.Rows.Count; i++)
            {
                var row = Result.Rows[i];
                Result.Rows[i] = new TextualRow(row, Result);
                Result.Rows[i].ValidateWithParent();
            }
        }

        public void FilterWhereExclusive(params int[] indices)
        {
            Result.Rows.Clear();
            foreach (var index in indices)
                Result.AddRow(table.GetRow(index));
        }
        public void FilterWhereExclusive(TextualWhereCondition condition)
        {
            for (int i = 0; i < Result.Rows.Count; i++)
                if (!condition.Check(this, Result.GetRow(i)))
                    Result.RemoveRow(i);
        }
        
        public void FilterWhereInclusive(params int[] indices)
        {
            foreach (var index in indices)
            {
                var row = table.GetRow(index);
                if (!Result.Rows.Contains(row))
                    Result.AddRow(row);
            }
        }
        public void FilterWhereInclusive(TextualWhereCondition condition)
        {
            foreach (var row in table.Rows)
                if (!Result.Rows.Contains(row))
                    if (condition.Check(this, row))
                        Result.AddRow(row);
        }
    }
}
