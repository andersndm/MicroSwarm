using MicroSwarm.FileSystem;
using System.Diagnostics;

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

                    argParser.PrintError(error);
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

                if (input.Files.Count == 0)
                {
                    argParser.PrintError("No input files specified.");
                    Environment.Exit(1);
                }

                List<SwarmFile> inputFiles = [];
                foreach (var filename in input.Files)
                {
                    try
                    {
                        inputFiles.Add(currentDir.GetFile(filename));
                    }
                    catch (DirectoryNotFoundException)
                    {
                        argParser.PrintError("Input file not found: " + filename);
                        Environment.Exit(1);
                    }
                    catch (FileNotFoundException)
                    {
                        argParser.PrintError("Input file not found: " + filename);
                        Environment.Exit(1);
                    }
                }
                Debug.Assert(inputFiles.Count > 0);

                SwarmDir? outputDir = null;
                if (input.OutputDir == null)
                {
                    outputDir = currentDir;
                }
                else
                {
                    try
                    {
                        outputDir = currentDir.GetDir(input.OutputDir);
                    }
                    catch (DirectoryNotFoundException)
                    {
                        argParser.PrintError("Invalid output directory: " + input.OutputDir);
                        Environment.Exit(1);
                    }
                }
                Debug.Assert(outputDir != null);
            }
        }
    }
}