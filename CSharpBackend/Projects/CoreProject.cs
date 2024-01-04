using MicroSwarm.FileSystem;

namespace CSharpBackend.Projects
{
    public class CoreProject : ClassLibraryProject
    {
        public static readonly string Suffix = "Core";

        public CoreProject(string serviceName, SwarmDir solutionDir) : base(serviceName + Suffix, solutionDir)
        {
        }
    }
}