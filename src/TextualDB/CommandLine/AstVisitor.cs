using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TextualDB.Components;
using TextualDB.CommandLine.Ast;

namespace TextualDB.CommandLine
{
    public class AstVisitor : IVisitor
    {
        public void Accept(FilterNode node)
        {

        }

        public void Accept(IdentifierNode node)
        {

        }
        
        public void Accept(ListNode node)
        {

        }

        public void Accept(SelectNode node)
        {

        }

        public void Accept(WhereNode node)
        {

        }
    }
}
