namespace MicroSwarm.Templates
{
    public static class EntityTemplate
    {
        public static string RenderHeader(string namespaceName, string entityName)
        {
            return
$$"""
namespace {{namespaceName}}
{
    public class {{entityName}}
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
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