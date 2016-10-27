using System;

namespace TextualDBD.Interpreter.Ast
{
    public class RenameColumnNode: AstNode
    {
        public string Column { get; private set; }
        public string Name { get; private set; }
        public string Table { get; private set; }

        public RenameColumnNode(string column, string name, string table)
        {
            Column = column;
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