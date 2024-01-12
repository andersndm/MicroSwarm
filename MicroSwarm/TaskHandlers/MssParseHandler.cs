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
            Array.Fill(parsers, new MssParser());
            if (!parsers[0].ValidateGrammar())
            {
                return IResult.BadResult("Grammar validation failed");
            }

            var tasks = new Task<MssSpecNode?>[input.Count()];
            for (int i = 0; i < input.Count(); ++i)
            {
                MssParser parser = parsers[i];
                SwarmFile inputFile = input.ElementAt(i);
                tasks[i] = Task.Run(() => parser.ParseMss(inputFile.Name, inputFile.LoadContent()));
            }

            Task.WhenAll(tasks).Wait();

            var errorsFound = false;
            foreach (var parser in parsers)
            {
                if (parser.HasErrors)
                {
                    foreach (var error in parser.Errors)
                    {
                        error.Print();
                    }
                    errorsFound = true;
                }
            }

            if (errorsFound)
            {
                return IResult.BadResult("Parsing had errors.");
            }
            // Todo: Remove once parsing is working properly!
            else
            {
                return IResult.OkResult("Successfully parsed.");
            }

            // return _next.Handle(tasks.Select(t => t.Result!));
        }
    }
}