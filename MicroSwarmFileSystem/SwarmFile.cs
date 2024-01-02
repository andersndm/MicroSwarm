namespace MicroSwarm.FileSystem
{
    public class SwarmFile(string name, SwarmDir dir)
    {
        protected string _name = name;
        protected SwarmDir _parent = dir;
    }
}