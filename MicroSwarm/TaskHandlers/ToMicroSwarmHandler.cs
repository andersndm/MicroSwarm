using MicroSwarm.FileSystem;
using Mss;
using MssBuilder;

namespace MicroSwarm.TaskHandlers
{
    public class ToMicroSwarmHandler(SwarmDir outDir) : TaskHandlerTail<MssSpec>
    {
        private readonly SwarmDir _outDir = outDir;

        public override IResult Handle(MssSpec input)
        {
            // Todo: name for the output? not as simple if allowing multiple files
            var solution = new MssCSharpSolution("swarm", _outDir, input);
            solution.Write();
            return IResult.OkResult("Built services");
        }
    }
}