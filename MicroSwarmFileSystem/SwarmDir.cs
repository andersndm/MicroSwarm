using System.Text;

namespace MicroSwarm.FileSystem
{
    public class SwarmDir
    {
        private string _name;
        private SwarmDir? _parent = null;

        private static readonly string _separator = OperatingSystem.IsWindows() ? "\\" : "/";

        public string Name { get => _name; }
        public SwarmDir? Parent { get => _parent; }
        public bool IsRoot { get => _parent == null; }

        public IEnumerable<SwarmDir> Children { get => GetChildren(); }

        public static SwarmDir CurrentDir { get => GetCurrentDirectory(); }

        public SwarmDir(string dirName)
        {
            _name = dirName;
        }

        public SwarmDir(string dirName, SwarmDir parent)
        {
            _name = dirName;
            _parent = parent;
        }

        public static SwarmDir GetCurrentDirectory()
        {
            string path = Directory.GetCurrentDirectory();
            var elements = path.Split(_separator);
            var dir = new SwarmDir(elements[0]);
            for (int i = 1; i < elements.Length; i++)
            {
                dir = new SwarmDir(elements[i], dir);
            }
            return dir;
        }

        public SwarmDir GetRoot()
        {
            if (IsRoot || _parent == null)
            {
                return this;
            }

            var parent = _parent;
            while (parent.Parent != null && !parent.IsRoot)
            {
                parent = parent.Parent;
            }
            return parent;
        }

        public string GetAbsolutePath()
        {
            StringBuilder builder = new();
            builder.Append(_name);
            builder.Append(_separator);
            SwarmDir? parent = _parent;
            while (parent != null)
            {
                builder.Insert(0, _separator);
                builder.Insert(0, parent.Name);
                parent = parent.Parent;
            }
            return builder.ToString();
        }

        public SwarmDir GetDir(string path)
        {
            var dir = this;

            string[] elements = path.Split(['/', '\\']);

            if (elements[0] != ".")
            {
                if (dir.GetRoot().Name != elements[0])
                {
                    if (Path.Exists(elements[0]))
                    {
                        dir = new SwarmDir(elements[0]);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException("Invalid path: " + path);
                    }
                }
                else
                {
                    dir = GetRoot();
                }
            }

            foreach (var element in elements[1..])
            {
                if (element == "") { continue; }
                else if (element == ".")
                {
                    throw new DirectoryNotFoundException("Unexpected '.' in path: " + path);
                }
                else if (element == "..")
                {
                    if (dir.IsRoot)
                    {
                        throw new DirectoryNotFoundException("Invalid path: " + path);
                    }
                    dir = dir.Parent!;
                }
                else
                {
                    dir = dir.GetChildren().Where(c => c.Name == element).FirstOrDefault() ??
                        throw new DirectoryNotFoundException("Invalid path: " + path);
                }
            }
            return dir;
        }

        public IEnumerable<SwarmFile> GetFiles()
        {
            var files = Directory.EnumerateFiles(GetAbsolutePath());
            return files.Select(f => new SwarmFile(f, this));
        }

        public IEnumerable<SwarmDir> GetChildren()
        {
            var dirs = Directory.EnumerateDirectories(GetAbsolutePath()).ToList();
            return dirs.Select(d => new SwarmDir(Path.GetFileName(d), this));
        }

        public override string ToString()
        {
            return GetAbsolutePath();
        }
    }
}