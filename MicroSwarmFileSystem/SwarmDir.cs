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
            if (Path.Exists(path))
            {
                string[] elements = path.Split(['/', '\\']);
                SwarmDir dir = GetPathRoot(elements[0]);
                return GetDir(dir, elements[1..]);
            }
            else
            {
                throw new DirectoryNotFoundException("Invalid path: " + path);
            }
        }

        public SwarmFile GetFile(string path)
        {
            string[] elements = path.Split(['/', '\\']);
            if (elements.Length == 1)
            {
                return GetFiles().FirstOrDefault(f => f.Name == elements[0]) ??
                    throw new FileNotFoundException(path);
            }

            if (Path.Exists(path))
            {
                string filename = elements.Last();
                SwarmDir dir = GetPathRoot(elements[0]);
                dir = GetDir(dir, elements[1..^1]);
                return dir.GetFiles().FirstOrDefault(f => f.Name == filename) ??
                    throw new FileNotFoundException(path);
            }
            else
            {
                throw new DirectoryNotFoundException("Invalid path: " + path);
            }
        }

        private SwarmDir GetPathRoot(string pathStart)
        {
            if (pathStart == ".")
            {
                return this;
            }
            else if (pathStart == "..")
            {
                return _parent ??
                    throw new DirectoryNotFoundException("Invalid path start: " + pathStart);
            }
            else
            {
                if (GetRoot().Name != pathStart)
                {
                    if (Path.Exists(pathStart))
                    {
                        return new SwarmDir(pathStart);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException("Invalid path start: " + pathStart);
                    }
                }
                else
                {
                    return GetRoot();
                }
            }
        }

        public SwarmFile CreateFile(string filename)
        {
            File.CreateText(GetAbsolutePath() + filename).Close();
            return new SwarmFile(filename, this);
        }

        private static SwarmDir GetDir(SwarmDir start, string[] path)
        {
            var dir = start;
            foreach (var element in path)
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
            return files.Select(f => new SwarmFile(Path.GetFileName(f), this));
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