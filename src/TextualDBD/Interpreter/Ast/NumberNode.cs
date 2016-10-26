using System;

namespace TextualDBD.Interpreter.Ast
{
    public class NumberNode: AstNode
    {
        public double Number { get; private set; }

        public NumberNode(double number)
        {
            Number = number;
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

