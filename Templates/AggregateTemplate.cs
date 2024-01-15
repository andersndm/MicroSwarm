namespace MicroSwarm.Templates
{
    public static class AggregateTemplate
    {
        public static string RenderHeader(string namespaceName, string entityName)
        {
            return
$$"""
namespace {{namespaceName}}.Aggregates
{
    public class {{entityName}}
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        public int Id { get; set; }
""";
        }

        public static string RenderFooter()
        {
            return
"""
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    }
}
""";
        }
    }
}