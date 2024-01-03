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
            // Todo: name for the output?
            MssSolutionBuilder.GenerateServices(input, _outDir, "swarm");
            return IResult.OkResult("Built services");
        }
    }
}