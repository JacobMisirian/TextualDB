using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDBD.Interpreter.Ast
{
    public class DropColumnNode: AstNode
    {
        public string Column { get; private set; }
        public string Table { get; private set; }

        public DropColumnNode(string table, string column)
        {
            Table = table;
            Column = column;
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
