namespace MicroSwarm.Templates
{
    public static class QueryFilterTemplate
    {
        public static string Render(string solutionName, string serviceName, string rootName)
        {
            return
$$"""
using {{solutionName}}Core.Aggregates;
using {{solutionName}}Core.Filtering;

namespace {{serviceName}}
{
    public class {{serviceName}}Filter : QueryFilter<{{rootName}}>
    {
        public override QueryFilter<{{rootName}}> FromJson(string jsonString)
        {
            return this;
        }
    }
}
""";
        }
    }
}