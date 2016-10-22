using System;

using TextualDBD.Interpreter.Ast;

namespace TextualDBD.Interpreter
{
    public interface IVisitor
    {
        void Accept(CreateColumnNode node);
        void Accept(DropColumnNode node);
        void Accept(DropTableNode node);
        void Accept(BinaryExpressionNode node);
        void Accept(CreateTableNode node);
        void Accept(IdentifierNode node);
        void Accept(InsertNode node);
        void Accept(SelectNode node);
        void Accept(StringNode node);
    }
}

