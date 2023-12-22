namespace MicroSwarm
{
    public class SwarmFile(string name, SwarmDir dir)
    {
        private string _name = name;
        private SwarmDir _parent = dir;
    }

    public class SwarmDir
    {
        private string _name;
        private SwarmDir? _parent = null;
        private static string _separator = OperatingSystem.IsWindows() ? "\\" : "/";

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

        private static SwarmDir GetCurrentDirectory()
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

        public string GetAbsolutePath()
        {
            string path = _name;
            SwarmDir? parent = _parent;
            while (parent != null)
            {
                path = parent.Name + _separator + path;
                parent = parent.Parent;
            }
            return path;
        }

        public string GetRelativePath()
        {
            return GetRelativePath(CurrentDir);
        }

        public string GetRelativePath(SwarmDir dir)
        {
            throw new NotImplementedException();
        }

        public SwarmDir GetRelativeDir(string path)
        {
            var dir = this;
            foreach (var element in path.Split(['/', '\\']))
            {
                if (element == "") { continue; }
                else if (element == ".")
                {
                    dir = GetCurrentDirectory();
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

        public IEnumerable<SwarmFile> GetFileNames()
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