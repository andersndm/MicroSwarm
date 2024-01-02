using Irony.Parsing;

namespace Mss.Parsing
{
    public class MssError(string msg, string filename, SourceLocation loc)
    {
        public string Message { get; } = msg;
        public string Filename { get; } = filename;
        public SourceLocation Location { get; set; } = loc;

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("MssError: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Filename);
            Console.ResetColor();
            Console.Write("(");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Location.Line + 1);
            Console.ResetColor();
            Console.Write(",");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(Location.Column + 1);
            Console.ResetColor();
            Console.WriteLine("): " + Message);
        }
    }
}