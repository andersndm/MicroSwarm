using Irony.Parsing;

namespace Mss.Parsing
{
    public class MssError(string msg, SourceLocation loc)
    {
        public string Message { get; } = msg;
        public SourceLocation Location { get; set; } = loc;
    }
}