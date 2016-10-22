using System;
using System.Collections.Generic;

namespace TextualDBD.Interpreter.Ast
{
    public class CreateTableNode: AstNode
    {
        public string Table { get; private set; }
        public List<string> Columns { get; private set; }

        public CreateTableNode(string table, List<string> columns)
        {
            Table = table;
            Columns = columns;
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

