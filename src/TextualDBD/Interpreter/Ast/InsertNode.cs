using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextualDBD.Interpreter.Ast
{
    public class InsertNode: AstNode
    {
        public string Table { get; private set; }
        public List<InsertValue> Values { get; private set; }
        public AstNode Where { get { return Children[0]; } }

        public InsertNode(string table, List<InsertValue> values, AstNode where)
        {
            Table = table;
            Values = values;
            Children.Add(where);
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

    public class InsertValue
    {
        public string Column { get; private set; }
        public string Value { get; private set; }

        public InsertValue(string column, string value)
        {
            Column = column;
            Value = value;
        }
    }
}
