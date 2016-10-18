using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDBD.Interpreter.Ast
{
    public class BinaryExpressionNode: AstNode
    {
        public BinaryExpressionType BinaryExpressionType { get; private set; }
        public AstNode Left { get { return Children[0]; } }
        public AstNode Right { get { return Children[1]; } }

        public BinaryExpressionNode(BinaryExpressionType type, AstNode left, AstNode right)
        {
            BinaryExpressionType = type;
            Children.Add(left);
            Children.Add(right);
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

    public enum BinaryExpressionType
    {
        And,
        Equality,
        GreaterThan,
        GreaterThanOrEqual,
        LesserThan,
        LesserThanOrEqual,
        NotEqual,
        Or
    }
}
