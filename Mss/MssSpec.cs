using Mss.Types;

namespace Mss
{
    public class MssSpec(IEnumerable<MssService> services, string filename)
    {
        private readonly List<MssService> _services = services.ToList();

        public IEnumerable<MssService> Services { get => _services; }

        public string Filename { get; } = filename;

        public bool Merge(MssSpec other)
        {
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