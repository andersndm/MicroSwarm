using MicroSwarm.FileSystem;
using MicroSwarm.Pipeline;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace MicroSwarm
{
    internal sealed class Program
    {
        private const string MAJOR_VERSION = "0";
        private const string MINOR_VERSION = "1";
        private const string PATCH_VERSION = "0";

        public static IEnumerable<SwarmFile>? GetInputFiles(IEnumerable<string> inputFiles, SwarmDir currentDir)
        {
            if (!inputFiles.Any())
            {
                SwarmArgParser.PrintError("No input files specified.");
                return null;
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
                    return null;
                }
                catch (FileNotFoundException)
                {
                    SwarmArgParser.PrintError("Input file not found: " + filename);
                    return null;
                }
            }
            Debug.Assert(result.Count > 0);
            return result;
        }

        public static SwarmDir? GetOutputDir(string? outputPath, SwarmDir currentDir)
        {
            if (outputPath == null)
            {
                return currentDir;
            }
            else
            {
                try
                {
                    return currentDir.GetDir(outputPath);
                }
                catch (DirectoryNotFoundException)
                {
                    SwarmArgParser.PrintError("Invalid output directory: " + outputPath);
                    return null;
                }
            }
        }

        public static int CMain(string[] args)
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
                return 1;
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

                IEnumerable<SwarmFile>? inputFiles = GetInputFiles(input.Files, currentDir);
                if (inputFiles == null)
                {
                    return 1;
                }
                SwarmDir? outputDir = GetOutputDir(input.OutputDir, currentDir);
                if (outputDir == null)
                {
                    return 1;
                }

                var pipeline = input.ToPuml switch
                {
                    true => PipelineFactory.CreatePumlPipeline(outputDir),
                    false => PipelineFactory.CreateMssPipeline(outputDir)
                };

                var result = pipeline.Execute(inputFiles);
                if (!result.Ok)
                {
                    Console.WriteLine("Bad result: " + result.Value);
                    return 1;
                }
            }

            return 0;
        }

        public static void Main(string[] args)
        {
            var returnValue = CMain(args);
            Environment.Exit(returnValue);
        }
    }
}