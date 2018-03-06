using System.Collections.Generic;

namespace TextualDB.Components.Operations
{
    public class TextualSelectOperation : TextualOperation
    {
        public override TextualTable Result { get; }
        
        private TextualTable table;

        private List<TextualRow> mirroredSourceRows;

        public TextualSelectOperation(TextualTable table, params string[] columns)
        {
            Result = new TextualTable(table.ParentDatabase, table.Name);
            this.table = table;

            if (columns.Length == 0)
                columns = table.Columns.ToArray();

            foreach (var column in columns)
                Result.AddColumn(column);

            mirroredSourceRows = new List<TextualRow>();
        }

        public void Execute()
        {
            if (mirroredSourceRows.Count == 0)
                foreach (var srcRow in table.Rows)
                    Result.AddRow(prepareRow(srcRow));
            foreach (var srcRow in mirroredSourceRows)
                Result.AddRow(prepareRow(srcRow));

            mirroredSourceRows = new List<TextualRow>();
        }

        private TextualRow prepareRow(TextualRow row)
        {
            TextualRow destRow = new TextualRow(Result);
            foreach (var column in Result.Columns)
                destRow.SetValue(column, row.GetValue(column));
            return destRow;
        }

        public void FilterWhereExclusive(int index)
        {
            FilterWhereInclusive(index);
            for (int i = 0; i < mirroredSourceRows.Count; i++)
            {
                var srcRow = mirroredSourceRows[i];
                if (table.Rows.IndexOf(srcRow) != index)
                    mirroredSourceRows.Remove(srcRow);
            }
        }
        public void FilterWhereExclusive(TextualWhereCondition condition)
        {
            FilterWhereInclusive(condition);
            for (int i = 0; i < mirroredSourceRows.Count; i++)
            {
                var srcRow = mirroredSourceRows[i];
                if (!condition.Check(this, srcRow))
                    mirroredSourceRows.Remove(srcRow);
            }
        }

        public void FilterWhereInclusive(int index)
        {
            mirroredSourceRows.Add(table.GetRow(index));
        }
        public void FilterWhereInclusive(TextualWhereCondition condition)
        {
            foreach (var srcRow in table.Rows)
                if (condition.Check(this, srcRow))
                    mirroredSourceRows.Add(srcRow);
        }
    }
}
