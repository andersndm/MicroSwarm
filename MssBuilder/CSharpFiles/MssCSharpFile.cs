using MicroSwarm.FileSystem;

namespace MssBuilder
{
    public class MssCSharpFile : SwarmFile
    {
        public MssCSharpFile(string filename, SwarmDir dir) : base(filename, dir) => _content = "";
        public MssCSharpFile(string filename, SwarmDir dir, string content) : base(filename, dir) => _content = content;

        protected string _content;

        public string Content { get => _content; }
        public void SetContent(string content)
        {
            if (content != null)
            {
                _content = content;
            }
        }

        public virtual void Write()
        {
            base.Write(_content);
        }
    }
}