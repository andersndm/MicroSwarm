using Mss;
using Mss.Ast;
using Mss.Resolver;

namespace MicroSwarm.TaskHandlers
{
    public class MssResolveHandler(ITaskHandler<MssSpec> next)
        : TaskHandler<IEnumerable<MssSpecNode>, MssSpec>(next)
    {
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
            return IResult.OkResult("");
            /*
            var resolver = new MssResolver();
            var root = parseTree.Root.AstNode as MssSpecNode ??
                throw new InvalidCastException("Unable to cast the root node to MssSpecNode");
            root.Accept(resolver);

            if (resolver.Errors.Count != 0)
            {
                foreach (var error in resolver.Errors)
                {
                    _errors.Add((filename, error));
                }
                return null;
            }

            return resolver.GetSpec();
            */
        }
    }
}