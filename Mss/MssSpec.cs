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
            // verify that any shared types are the same
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
                    else
                    {
                        typesToAdd.Add(otherType);
                    }
                }
            }
            _types.AddRange(typesToAdd);

            // verify that no services have the same name
            foreach (var otherService in other.Services)
            {
                foreach (var service in _services)
                {
                    if (service.Name == otherService.Name)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Mss Merge Error: ");
                        Console.ResetColor();
                        Console.WriteLine("Conflicting services with the same name, '" + service.Name + "' found in ");
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
            _services.AddRange(other.Services);

            return true;
        }
    }
}