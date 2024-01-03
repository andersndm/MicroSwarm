using Mss;
using Mss.Ast;
using Mss.Resolver;
using System.Diagnostics;

namespace MicroSwarm.TaskHandlers
{
    public class MssResolveHandler(ITaskHandler<IEnumerable<MssSpec>> next)
        : TaskHandler<IEnumerable<MssSpecNode>, IEnumerable<MssSpec>>(next)
    {
        // Todo: move to a merger handler
        /*
        private static MssSpec? MergeSpecs(IEnumerable<MssSpec> specs)
        {
            Debug.Assert(specs.Any());
            var master = specs.First();
            foreach (var spec in specs.ToArray()[1..])
            {
                if (!master.Merge(spec))
                {
                    return null;
                }
            }
            return master;
        }
        */

        public override IResult Handle(IEnumerable<MssSpecNode> input)
        {
            if (!input.Any())
            {
                return IResult.BadResult("No spec nodes supplied");
            }

            var resolvers = new MssResolver[input.Count()];
            var tasks = new Task[input.Count()];
            for (int i = 0; i < input.Count(); ++i)
            {
                var node = input.ElementAt(i);
                resolvers[i] = new(node.Filename);
                var resolver = resolvers[i];
                tasks[i] = Task.Run(() => node.Accept(resolver));
            }

            Task.WhenAll(tasks).Wait();

            var errorsFound = false;
            foreach (var resolver in resolvers)
            {
                if (resolver.Errors.Count != 0)
                {
                    foreach (var error in resolver.Errors)
                    {
                        error.Print();
                    }
                    errorsFound = true;
                }
            }

            if (errorsFound)
            {
                return IResult.BadResult("Errors found while resolving");
            }

            return _next.Handle(resolvers.Select(r => r.GetSpec()));
        }
    }
}