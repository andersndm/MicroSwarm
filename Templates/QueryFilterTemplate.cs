namespace MicroSwarm.Templates
{
    public static class QueryFilterTemplate
    {
        public static string Render(string solutionName, string serviceName)
        {
            return
$$"""
using {{serviceName}}.Entities;
using {{solutionName}}Core.Filtering;

namespace {{serviceName}}
{
    public class {{serviceName}}Filter : QueryFilter<{{serviceName}}Root>
    {
        public override QueryFilter<{{serviceName}}Root> FromJson(string jsonString)
        {
            return this;
        }
    }
}
""";
        }
    }
}