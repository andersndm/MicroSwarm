using CSharpBackend.Files;
using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;

namespace CSharpBackend.Projects
{
    public class CoreProject : ClassLibraryProject
    {
        public static readonly string Suffix = "Core";

        public CoreProject(string solutionName, MssSpec spec, SwarmDir solutionDir) : base(solutionName + Suffix, solutionDir)
        {
            var aggregateDir = Dir.CreateSub("Aggregates", true);
            foreach (var service in spec.Services)
            {
                var aggClass = new AggregateClassFile(solutionName + Suffix, service.Name, aggregateDir, service.AggregateFields);
                AddFile(aggClass);
            }

            // add filter classes
            var filterDir = Dir.CreateSub("Filtering", true);
            AddFile(new CSharpFile("QueryFilter", filterDir, QueryFilterBaseClassTemplate.Render(solutionName)));

            // add actor classes
            var actorDir = Dir.CreateSub("Actors", true);
            AddFile(new CSharpFile("IActorResult", actorDir, IActorResultTemplate.Render(solutionName)));
            AddFile(new CSharpFile("IActorBridge", actorDir, IActorBridgeTemplate.Render(solutionName)));
            AddFile(new CSharpFile("ActorService", actorDir, ActorServiceTemplate.Render(solutionName)));

            AddPackageReference("Microsoft.Extensions.Hosting", "8.0.0");
            AddPackageReference("Akka", "1.5.14");
            AddPackageReference("Akka.DependencyInjection", "1.5.14");
        }
    }
}