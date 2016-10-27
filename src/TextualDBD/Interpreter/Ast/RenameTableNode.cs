using System;

namespace TextualDBD.Interpreter.Ast
{
    public class RenameTableNode: AstNode
    {
        public string Name { get; private set; }
        public string Table { get; private set; }

        public RenameTableNode(string name, string table)
        {
            Name = name;
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

