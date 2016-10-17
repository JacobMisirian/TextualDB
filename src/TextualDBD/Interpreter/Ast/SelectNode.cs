using System;

namespace TextualDBD.Interpreter.Ast
{
    public class SelectNode: AstNode
    {
        public string Column { get; private set; }
        public string Table { get; private set; }
        public AstNode Where { get { return Children[0]; } }

        public SelectNode(string column, string table, AstNode where)
        {
            Column = column;
            Table = table;
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

