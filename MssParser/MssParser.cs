using Irony.Parsing;
using Mss.Ast;

namespace Mss.Parsing
{
    public class MssParser
    {
        private readonly MssGrammar _grammar = new();
        private readonly List<(string, MssError)> _errors = [];

        public bool HasErrors { get => _errors.Count != 0; }

        public void PrintErrors()
        {
            foreach (var (filename, error) in _errors)
            {
                PrintErrorWithLocation(filename, error.Location, error.Message);
            }
        }

        private static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }

        private static void PrintErrorWithLocation(string filename, SourceLocation location, string message)
        {
            PrintErrorWithLocation(filename, location.Line + 1, location.Column + 1, message);
        }

        private static void PrintErrorWithLocation(string filename, int line, int column, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(filename);
            Console.ResetColor();
            Console.Write("(");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(line);
            Console.ResetColor();
            Console.Write(",");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(column);
            Console.ResetColor();
            Console.WriteLine("): " + message);
        }

        public bool ValidateGrammar()
        {
            var language = new LanguageData(_grammar);
            if (language.Errors.Count > 0)
            {
                foreach (var error in language.Errors.AsEnumerable())
                {
                    PrintError("in grammar: " + error.ToString());
                }
                return false;
            }
            return true;
        }

        public MssSpecNode? ParseMss(string filename, string mssSource)
        {
            var parser = new Parser(_grammar);
            var parseTree = parser.Parse(mssSource, filename);
            if (parseTree.HasErrors())
            {
                foreach (var error in parseTree.ParserMessages)
                {
                    _errors.Add((filename, new(error.Message, error.Location)));
                }
                return null;
            }

            return parseTree.Root.AstNode as MssSpecNode;

            /*
            var resolver = new MssResolver();
            var root = parseTree.Root.AstNode as MssSpecNode ??
                throw new InvalidCastException("Unable to cast the root node to MssSpecNode");
            root.Accept(resolver);

            if (resolver.Errors.Count != 0)
            {
                foreach (var error in resolver.Errors)
                {
                    _errors.Add((filename, error));
                }
                return null;
            }

            return resolver.GetSpec();
            */
        }
    }
}