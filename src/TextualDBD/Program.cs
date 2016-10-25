using System;

using TextualDBD.Interfaces;

namespace TextualDBD
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new TextualDBDArgumentParser().Parse(args).Main();
        }
    }
}
