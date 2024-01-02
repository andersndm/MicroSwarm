using MicroSwarm.FileSystem;
using System.Diagnostics;

namespace MicroSwarm
{
    internal sealed class Program
    {
        private const string MAJOR_VERSION = "0";
        private const string MINOR_VERSION = "1";
        private const string PATCH_VERSION = "0";

        public static IEnumerable<SwarmFile> GetInputFiles(IEnumerable<string> inputFiles, SwarmDir currentDir)
        {
            if (!inputFiles.Any())
            {
                SwarmArgParser.PrintError("No input files specified.");
                Environment.Exit(1);
            }

            List<SwarmFile> result = [];
            foreach (var filename in inputFiles)
            {
                try
                {
                    result.Add(currentDir.GetFile(filename));
                }
                catch (DirectoryNotFoundException)
                {
                    SwarmArgParser.PrintError("Input file not found: " + filename);
                    Environment.Exit(1);
                }
                catch (FileNotFoundException)
                {
                    SwarmArgParser.PrintError("Input file not found: " + filename);
                    Environment.Exit(1);
                }
            }
            Debug.Assert(result.Count > 0);
            return result;
        }

        public static SwarmDir GetOutputDir(string? outputPath, SwarmDir currentDir)
        {
            SwarmDir? result = null;
            if (outputPath == null)
            {
                result = currentDir;
            }
            else
            {
                try
                {
                    result = currentDir.GetDir(outputPath);
                }
                catch (DirectoryNotFoundException)
                {
                    SwarmArgParser.PrintError("Invalid output directory: " + outputPath);
                    Environment.Exit(1);
                }
            }
            Debug.Assert(result != null);
            return result;
        }

        public static void Main(string[] args)
        {
            string programInvocation = "dotnet run";

            SwarmArgParser argParser = new();
            SwarmInput input = argParser.Parse(args);

            if (input.Errors.Count != 0)
            {
                foreach (var error in input.Errors)
                {
                    SwarmArgParser.PrintError(error);
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

                IEnumerable<SwarmFile> inputFiles = GetInputFiles(input.Files, currentDir);
                SwarmDir outputDir = GetOutputDir(input.OutputDir, currentDir);

            }
        }
    }
}