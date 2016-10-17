using System;
using System.Collections.Generic;

namespace TextualDBD.Interpreter.Ast
{
    public abstract class AstNode
    {
        public List<AstNode> Children = new List<AstNode>();
        public abstract void Visit(IVisitor visitor);
        public abstract void VisitChildren(IVisitor visitor);
    }
}

