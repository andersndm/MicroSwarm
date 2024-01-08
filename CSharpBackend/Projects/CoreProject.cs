using CSharpBackend.Files;
using MicroSwarm.FileSystem;
using MicroSwarm.Templates;

namespace CSharpBackend.Projects
{
    public class CoreProject : ClassLibraryProject
    {
        public static readonly string Suffix = "Core";

        public CoreProject(string serviceName, SwarmDir solutionDir) : base(serviceName + Suffix, solutionDir)
        {
            // add filter classes
            var filterDir = Dir.CreateSub("Filtering", true);
            AddFile(new CSharpFile("QueryFilter", filterDir, QueryFilterBaseClassTemplate.Render(serviceName)));

            // add actor classes
            var actorDir = Dir.CreateSub("Actors", true);
            AddFile(new CSharpFile("IActorResult", actorDir, IActorResultTemplate.Render(serviceName)));
            AddFile(new CSharpFile("IActorBridge", actorDir, IActorBridgeTemplate.Render(serviceName)));
            AddFile(new CSharpFile("ActorService", actorDir, ActorServiceTemplate.Render(serviceName)));

            AddPackageReference("Microsoft.Extensions.Hosting", "8.0.0");
            AddPackageReference("Akka", "1.5.14");
            AddPackageReference("Akka.DependencyInjection", "1.5.14");
        }
    }
}