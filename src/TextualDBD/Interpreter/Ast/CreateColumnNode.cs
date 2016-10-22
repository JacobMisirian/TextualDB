using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDBD.Interpreter.Ast
{
    public class CreateColumnNode: AstNode
    {
        public string Column { get; private set; }
        public string Table { get; private set; }
        public int Position { get; private set; }

        public CreateColumnNode(string table, string column, int position)
        {
            Table = table;
            Column = column;
            Position = position;
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
