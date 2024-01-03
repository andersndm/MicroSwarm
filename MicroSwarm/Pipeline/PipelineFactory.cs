using MicroSwarm.FileSystem;
using MicroSwarm.TaskHandlers;

namespace MicroSwarm.Pipeline
{
    public static class PipelineFactory
    {
        public static IPipeline<IEnumerable<SwarmFile>> CreatePumlPipeline(SwarmDir outDir)
        {
            ToPumlHandler pumlHandler = new(outDir);
            MssResolveHandler resolveHandler = new(pumlHandler);
            MssParseHandler parseHandler = new(resolveHandler);
            return new Pipeline<IEnumerable<SwarmFile>>(parseHandler);
        }
    }
}