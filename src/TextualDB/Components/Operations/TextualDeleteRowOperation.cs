using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDB.Components.Operations
{
    public class TextualDeleteRowOperation : TextualOperation
    {
        public override TextualTable Result => throw new NotImplementedException();

        private TextualTable table;

        private List<TextualRow> mirroredSourceRows;

        public TextualDeleteRowOperation(TextualTable table)
        {
            this.table = table;

            mirroredSourceRows = new List<TextualRow>();
        }

        public void Execute(int index)
        {
            table.RemoveColumn(index);
        }
        public void Execute()
        {
            foreach (var row in mirroredSourceRows)
                table.RemoveRow(row);
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
                if (condition.Check(this, srcRow))
                    mirroredSourceRows.Add(srcRow);
        }
    }
}
