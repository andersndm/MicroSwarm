using MicroSwarm.FileSystem;

namespace MicroSwarm
{
    internal sealed class Program
    {
        private const string MAJOR_VERSION = "0";
        private const string MINOR_VERSION = "1";
        private const string PATCH_VERSION = "0";

        public static void Main(string[] args)
        {
            string programInvocation = "dotnet run";

            SwarmArgParser argParser = new();
            SwarmInput input = argParser.Parse(args);

            if (input.Errors.Count != 0)
            {
                foreach (var error in input.Errors)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.Write("Input Error: ");
                    Console.ResetColor();
                    Console.WriteLine(error);
                }

                argParser.PrintUsage(programInvocation);
                Environment.Exit(1);
            }

            if (input.HelpRequested)
            {
                argParser.PrintUsage(programInvocation);
            }
            else if (input.VersionRequested)
            {
                Console.WriteLine($"MicroSwarm version {MAJOR_VERSION}.{MINOR_VERSION}.{PATCH_VERSION}");
            }
            else
            {
                var currentDir = SwarmDir.CurrentDir;
                Console.WriteLine("current dir: " + currentDir);
                var outputDir = input.OutputDir == null ? currentDir : currentDir.GetDir(input.OutputDir);
            }
        }
    }
}