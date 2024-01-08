using CSharpBackend.Projects;
using MicroSwarm.FileSystem;
using MicroSwarm.Templates;

namespace CSharpBackend.Files
{
    public class QueryRootFilterFile : CSharpFile
    {
        public QueryRootFilterFile(string solutionName, string serviceName, SwarmDir dir, bool usesValues)
            : base(serviceName + "Filter", dir)
        {
            if (usesValues)
            {
                AppendLine(UsingTemplate.Render(ValueTypeProject.ProjectName));
                AppendLine();
            }
            Append(QueryFilterTemplate.Render(solutionName, serviceName));
        }
    }
}