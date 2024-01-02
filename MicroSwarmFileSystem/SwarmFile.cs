namespace MicroSwarm.FileSystem
{
    public class SwarmFile(string name, SwarmDir dir)
    {
        protected string _name = name;
        protected SwarmDir _parent = dir;

        public string Name { get => _name; }
        public SwarmDir Parent { get => _parent; }

        public string LoadContent()
        {
            return File.ReadAllText(_parent.ToString() + _name);
        }
    }
}