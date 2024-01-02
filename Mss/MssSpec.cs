using Mss.Types;

namespace Mss
{
    public class MssSpec(IEnumerable<MssType> types, IEnumerable<MssService> services, string filename)
    {
        private readonly List<MssType> _types = types.ToList();
        private readonly List<MssService> _services = services.ToList();

        public IEnumerable<MssType> Types { get => _types; }
        public IEnumerable<MssService> Services { get => _services; }

        public string Filename { get; } = filename;

        public bool Merge(MssSpec other)
        {
            List<MssType> typesToAdd = [];
            foreach (var otherType in other.Types)
            {
                foreach (var type in _types)
                {
                    if (type.ToString() == otherType.ToString())
                    {
                        if (!type.IsSameType(otherType))
                        {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Mss Merge Error: ");
                            Console.ResetColor();
                            Console.WriteLine("Conflicting types with the same name, '" + type.ToString() + "' found in ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(Filename);
                            Console.ResetColor();
                            Console.Write(" and ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(other.Filename);
                            Console.ResetColor();
                            return false;
                        }
                    }
                }
                typesToAdd.Add(otherType);
            }

            _types.AddRange(typesToAdd);
            _services.AddRange(other.Services);

            return true;
        }
    }
}