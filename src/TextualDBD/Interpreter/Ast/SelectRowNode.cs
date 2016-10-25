using System;

namespace TextualDBD.Interpreter.Ast
{
    public class SelectRowNode: AstNode
    {
        public string Table { get; private set; }
        public string Column { get; private set; }
        public int RowStart { get; private set; }
        public int RowEnd { get; private set; }
        public bool UseRowRange { get { return RowEnd != -1; } }
        public AstNode Where { get { return Children[0]; } }

        public SelectRowNode(string table, string column, int rowStart, int rowEnd, AstNode where)
        {
            Table = table;
            Column = column;
            RowStart = rowStart;
            RowEnd = rowEnd;
            Children.Add(where);
        }

        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
        public override void VisitChildren(IVisitor visitor)
        {
            foreach (var child in Children)
                child.Visit(visitor);
        }
    }
}

