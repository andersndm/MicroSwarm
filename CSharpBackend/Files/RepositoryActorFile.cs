using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;
using System.Diagnostics;

namespace CSharpBackend.Files
{
    public class RepositoryActorFile : CSharpFile
    {
        public const string CLASS_NAME = "RepositoryActor";

        public RepositoryActorFile(string solutionName, MssService service, SwarmDir dir)
            : base(CLASS_NAME, dir)
        {
            Append(RepositoryActorTemplate.Render(solutionName, service.Name, AggregateClassFile.GetName(service.Name)));
        }
    }
}