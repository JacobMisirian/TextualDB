using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDBD.Interpreter.Ast
{
    public class ShowColumnsNode: AstNode
    {
        public string Table { get; private set; }

        public ShowColumnsNode(string table)
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
