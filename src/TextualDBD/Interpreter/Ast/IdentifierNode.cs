using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDBD.Interpreter.Ast
{
    public class IdentifierNode: AstNode
    {
        public string Identifier { get; private set; }

        public IdentifierNode(string identifier)
        {
            Identifier = identifier;
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
