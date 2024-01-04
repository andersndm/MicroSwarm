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
""";
        }

        public static string RenderFooter()
        {
            return
"""
    }
}
""";
        }
    }
}