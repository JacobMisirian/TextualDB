using TextualDBC.Interfaces;

namespace TextualDBC
{
    class Program
    {
        static void Main(string[] args)
        {
            TUI.StartTUI(new TextualDBCArgumentParser().Parse(args));
        }
    }
}
