using System;
using System.Collections.Generic;

using TextualDBD.Interpreter.Ast;

namespace TextualDBD.Interpreter
{
    public class Parser
    {
        public List<Token> Tokens { get; private set; }
        private int position;

        public AstNode Parse(List<Token> tokens)
        {
            Tokens = tokens;
            position = 0;
        }

        private AstNode parseStatement()
        {
        }

        private SelectNode parseSelect()
        {

        }

        private AstNode parseExpression()
        {

        }


    }
}