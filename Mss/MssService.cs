namespace Mss
{
    public class MssService(string name, MssRoot root)
    {
        public string Name { get; } = name;
        public MssRoot Root { get; } = root;
    }
}