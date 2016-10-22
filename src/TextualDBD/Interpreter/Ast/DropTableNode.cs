using System;

namespace TextualDBD.Interpreter.Ast
{
    public class DropTableNode: AstNode
    {
        public string Table { get; private set; }

        public DropTableNode(string table)
        {
            Table = table;
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

