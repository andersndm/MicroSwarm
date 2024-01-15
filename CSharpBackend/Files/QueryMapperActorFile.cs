using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;

namespace CSharpBackend.Files
{
    public class QueryMapperActorFile : CSharpFile
    {
        public const string CLASS_NAME = "QueryMapperActor";

        public QueryMapperActorFile(string solutionName, MssService service, SwarmDir dir)
            : base(CLASS_NAME, dir)
        {
            AppendLine(QueryMapperActorTemplate.Render(solutionName, service.Name, AggregateClassFile.GetName(service.Name)));
        }
    }
}