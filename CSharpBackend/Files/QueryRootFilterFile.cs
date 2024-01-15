using MicroSwarm.FileSystem;
using MicroSwarm.Templates;

namespace CSharpBackend.Files
{
    public class QueryRootFilterFile : CSharpFile
    {
        public QueryRootFilterFile(string solutionName, string serviceName, SwarmDir dir)
            : base(serviceName + "Filter", dir)
        {
            Append(QueryFilterTemplate.Render(solutionName, serviceName, AggregateClassFile.GetName(serviceName)));
        }
    }
}