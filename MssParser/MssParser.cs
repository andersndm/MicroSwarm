using Irony.Parsing;
using Mss.Ast;

namespace Mss.Parsing
{
    public class MssParser
    {
        private readonly MssGrammar _grammar = new();
        private readonly List<MssError> _errors = [];

        public bool HasErrors { get => _errors.Count != 0; }
        public IEnumerable<MssError> Errors { get => _errors; }

        private static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Error: ");
            Console.ResetColor();
            Console.WriteLine(message);
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
                    _errors.Add(new(error.Message, filename, error.Location));
                }
                return null;
            }

            return parseTree.Root.AstNode as MssSpecNode;
        }
    }
}