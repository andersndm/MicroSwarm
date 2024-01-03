using Mss;
using System.Diagnostics;

namespace MicroSwarm.TaskHandlers
{
    public class MergeSpecsHandler(ITaskHandler<MssSpec> next)
        : TaskHandler<IEnumerable<MssSpec>, MssSpec>(next)
    {
        public override IResult Handle(IEnumerable<MssSpec> input)
        {
            Debug.Assert(input.Any());
            if (input.Count() == 1)
            {
                return _next.Handle(input.ElementAt(0));
            }

            var master = input.First();
            foreach (var spec in input.ToArray()[1..])
            {
                if (!master.Merge(spec))
                {
                    return IResult.BadResult("Failed to merge specs");
                }
            }
            return _next.Handle(master);
        }
    }
}