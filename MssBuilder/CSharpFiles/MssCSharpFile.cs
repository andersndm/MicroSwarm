using MicroSwarm.FileSystem;
using System.Text;

namespace MssBuilder
{
    public class MssCSharpFile : SwarmFile
    {
        private readonly StringBuilder _builder = new();
        private int _indent = 0;
        private readonly string _singleIndentation = "    ";
        private const string EXTENSION = ".cs";

        public MssCSharpFile(string className, SwarmDir dir) : base(className + EXTENSION, dir) { }
        public MssCSharpFile(string className, SwarmDir dir, string content)
            : base(className + EXTENSION, dir) => _builder.Append(content);

        public string Content { get => _builder.ToString(); }

        public void Indent() => ++_indent;
        public void Indent(int count)
        {
            if (_indent + count > 0)
            {
                _indent += count;
            }
            else
            {
                _indent = 0;
            }
        }

        public void UnIndent()
        {
            if (_indent > 0)
            {
                --_indent;
            }
        }

        public void ClearIndentation() => _indent = 0;

        private void ApplyIndentation()
        {
            for (int i = 0; i < _indent; i++)
            {
                _builder.Append(_singleIndentation);
            }
        }

        public void Append(string str)
        {
            var lines = str.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length - 1; i++)
            {
                ApplyIndentation();
                _builder.AppendLine(lines[i]);
            }
            ApplyIndentation();
            _builder.Append(lines.Last());
        }

        public void AppendLine(string line)
        {
            var lines = line.Split(Environment.NewLine);
            foreach (var newline in lines)
            {
                ApplyIndentation();
                _builder.AppendLine(newline);
            }
        }

        public void Prepend(string str)
        {
            string currentContent = _builder.ToString();
            _builder.Clear();
            _builder.Append(str);
            _builder.Append(currentContent);
        }

        public void PrependLine(string line)
        {
            string currentContent = _builder.ToString();
            _builder.Clear();
            _builder.AppendLine(line);
            _builder.Append(currentContent);
        }

        public virtual void Write()
        {
            base.Write(_builder.ToString());
        }
    }
}