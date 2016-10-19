using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextualDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new TextualDBReader(args[0]).Read();
            new TextualDBWriter().Write(db, args[1]);
        }
    }
}
