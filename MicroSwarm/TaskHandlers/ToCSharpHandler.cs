using MicroSwarm.FileSystem;
using Mss;
// using CSharpBackend;

namespace MicroSwarm.TaskHandlers
{
    public class ToCSharpHandler(SwarmDir outDir) : TaskHandlerTail<MssSpec>
    {
        private readonly SwarmDir _outDir = outDir;

        public override IResult Handle(MssSpec input)
        {
            // Todo: name for the output? not as simple if allowing multiple files
            // var solution = new CSharpSolution("swarm", _outDir, input);
            // solution.Write();
            return IResult.OkResult("Built services");
        }
    }
}