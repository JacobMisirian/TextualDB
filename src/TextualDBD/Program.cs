using System;

using TextualDBD.Interpreter;
using TextualDBD.Interpreter.Ast;

namespace TextualDBD
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var ast = new Parser().Parse(new Tokenizer().Scan("select Names from People"));
            
        }

        static int indent = 0;
        static void printAst(AstNode node)
        {
            for (int i = 0; i < indent; i++) Console.Write("   ");
            Console.WriteLine(node != null ? node.GetType().Name : "null");
            indent++;
            foreach (AstNode child in node.Children)
                printAst(child);
            indent--;
        }
    }
}
