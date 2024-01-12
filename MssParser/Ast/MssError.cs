using Irony.Parsing;

namespace Mss.Parsing
{
    public class MssError(string message, string filename, SourceLocation location)
    {
        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("MssError: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(filename);
            Console.ResetColor();
            Console.Write("(");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(location.Line + 1);
            Console.ResetColor();
            Console.Write(",");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(location.Column + 1);
            Console.ResetColor();
            Console.WriteLine("): " + message);
        }
    }
}