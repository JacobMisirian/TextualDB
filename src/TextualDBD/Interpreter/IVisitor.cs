using System;

using TextualDBD.Interpreter.Ast;

namespace TextualDBD.Interpreter
{
    public interface IVisitor
    {
        void Accept(DropNode node);
        void Accept(CreateNode node);
        void Accept(SelectNode node);
    }
}

