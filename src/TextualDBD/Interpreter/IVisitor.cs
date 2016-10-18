using System;

using TextualDBD.Interpreter.Ast;

namespace TextualDBD.Interpreter
{
    public interface IVisitor
    {
        void Accept(DropNode node);
        void Accept(BinaryExpressionNode node);
        void Accept(CreateNode node);
        void Accept(IdentifierNode node);
        void Accept(SelectNode node);
        void Accept(StringNode node);
    }
}

