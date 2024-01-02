using Mss;
using Mss.Ast;
using Mss.Resolver;
using System.Diagnostics;

namespace MicroSwarm.TaskHandlers
{
    public class MssResolveHandler(ITaskHandler<MssSpec> next)
        : TaskHandler<IEnumerable<MssSpecNode>, MssSpec>(next)
    {
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

        public override IResult Handle(IEnumerable<MssSpecNode> input)
        {
            if (!input.Any())
            {
                return IResult.BadResult("No spec nodes supplied");
            }

            var resolvers = new MssResolver[input.Count()];
            List<Task> tasks = [];
            int resolverIndex = 0;
            foreach (var node in input)
            {
                resolvers[resolverIndex] = new(node.Filename);
                tasks.Add(Task.Run(() => node.Accept(resolvers[resolverIndex])));
                ++resolverIndex;
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

            var output = MergeSpecs(resolvers.Select(r => r.GetSpec()));
            if (output != null)
            {
                return _next.Handle(output);
            }
            else
            {
                return IResult.BadResult("Unable to merge specs");
            }
        }
    }
}