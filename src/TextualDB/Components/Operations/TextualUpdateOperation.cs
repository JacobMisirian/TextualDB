using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.Components.Operations.Exceptions;

namespace TextualDB.Components.Operations
{
    public class TextualUpdateOperation : TextualOperation
    {
        public override TextualTable Result { get; }

        private TextualTable table;
        private string[] columns;
        private object[] values;

        private List<TextualRow> mirroredSourceRows;

        public TextualUpdateOperation(TextualTable table, string[] columns, object[] values)
        {
            this.table = table;
            this.columns = columns;
            this.values = values;

            mirroredSourceRows = new List<TextualRow>();

            foreach (var srcRow in table.Rows)
                mirroredSourceRows.Add(srcRow);

            if (columns.Length != values.Length)
                throw new ColumnValueCountMismatchException(this, columns.Length, values.Length);

            Result = table;
        }

        public void Execute(int index)
        {
            var row = table.GetRow(index);
            for (int i = 0; i < columns.Length; i++)
                row.SetValue(columns[i], values[i]);
        }
        public void Execute()
        {
            foreach (var destRow in mirroredSourceRows)
                for (int i = 0; i < columns.Length; i++)
                    destRow.SetValue(columns[i], values[i]);
        }

        public void FilterWhereExclusive(TextualWhereCondition condition)
        {
            for (int i = 0; i < mirroredSourceRows.Count; i++)
            {
                var srcRow = mirroredSourceRows[i];
                if (!condition.Check(this, srcRow))
                    mirroredSourceRows.Remove(srcRow);
            }
        }

        public void FilterWhereInclusive(TextualWhereCondition condition)
        {
            foreach (var srcRow in table.Rows)
                if (condition.Check(this, srcRow) && !mirroredSourceRows.Contains(srcRow))
                    mirroredSourceRows.Add(srcRow);
        }
    }
}
