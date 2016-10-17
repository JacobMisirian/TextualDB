using System;

namespace TextualDBD.Interpreter.Ast
{
    public class DropNode: AstNode
    {
        public string Table { get; private set; }

        public DropNode(string table)
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

