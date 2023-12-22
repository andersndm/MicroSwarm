using MicroSwarm.Args;

namespace MicroSwarm
{
    public class SwarmArgParser : ArgParser<SwarmInput>
    {
        public SwarmArgParser()
        {
            ArgOption<SwarmInput> help = new('h', "help", "Show this message.", (input, args) =>
            {
                input.HelpRequested = true;
                return 0;
            });

            ArgOption<SwarmInput> version = new('v', "version", "Prints out the MicroSwarm version number.", (input, args) =>
            {
                input.VersionRequested = true;
                return 0;
            });

            ArgOption<SwarmInput> output = new('o', "output", "Output directory, defaults to the current directory.",
                                               (input, args) =>
            {
                if (args.Length > 1)
                {
                    if (input.OutputDir == null)
                    {
                        input.OutputDir = args[1];
                    }
                    else
                    {
                        input.Errors.Add("Multiple output directories specified.");
                    }
                    return 1;
                }
                else
                {
                    input.Errors.Add("No output directory specified.");
                    return 0;
                }
            });

            ArgOption<SwarmInput> toPuml = new(null, "to-plantuml", "Generate a PlantUML diagram of the spec.", (input, args) =>
            {
                input.ToPuml = true;
                return 0;
            });

            AddOption(help);
            AddOption(version);
            AddOption(output);
            AddOption(toPuml);
        }

        public void PrintUsage(string invocation)
        {
            Console.WriteLine("Usage: " + invocation + " -- [options] <input file(s)>\n");
            base.PrintOptions();
        }
    }
}