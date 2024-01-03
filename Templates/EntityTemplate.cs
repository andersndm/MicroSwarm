namespace CLI.Templates
{
    public static class EntityTemplate
    {
        public static string Render(string namespaceName, string entityName, string properties)
        {
            return
$$"""
namespace {{namespaceName}}
{
    public class {{entityName}}
    {
{{properties}}
    }
}
""";
        }
    }
}