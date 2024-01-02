using MicroSwarm.FileSystem;
using Mss.Ast;
using Mss.Parsing;

namespace MicroSwarm.TaskHandlers
{
    public class MssParseHandler(ITaskHandler<IEnumerable<MssSpecNode>> next)
        : TaskHandler<IEnumerable<SwarmFile>, IEnumerable<MssSpecNode>>(next)
    {
        public override IResult Handle(IEnumerable<SwarmFile> input)
        {
            if (!input.Any())
            {
                return IResult.BadResult("No input files specified");
            }

            var parsers = new MssParser[input.Count()];
            if (!parsers[0].ValidateGrammar())
            {
                return IResult.BadResult("Grammar validation failed");
            }

            List<Task<MssSpecNode?>> tasks = [];
            int parserIndex = 0;
            foreach (var file in input)
            {
                tasks.Add(Task.Run(() => parsers[parserIndex].ParseMss(file.Name, file.LoadContent())));
                ++parserIndex;
            }

            Task.WhenAll(tasks).Wait();

            var errorsFound = false;
            foreach (var parser in parsers)
            {
                if (parser.HasErrors)
                {
                    parser.PrintErrors();
                    errorsFound = true;
                }
            }

            if (errorsFound)
            {
                return IResult.BadResult("Parsing had errors.");
            }

            List<MssSpecNode> output = [];
            foreach (var task in tasks)
            {
                output.Add(task.Result!);
            }

            return _next.Handle(output);
        }
    }
}