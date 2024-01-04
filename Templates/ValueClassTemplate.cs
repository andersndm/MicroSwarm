namespace MicroSwarm.Templates
{
    public static class ValueClassTemplate
    {
        private const string TEMPLATE_STR =
@"namespace {{Namespace}}
{
    public class {{ClassName}}
    {
        public {{TypeName}} {{PropName}} { get; set; }

        public {{ClassName}}() { }
        public {{ClassName}}({{TypeName}} value) => {{PropName}} = value;
    }
}";

        public static string Render(string namespaceName, string className, string fieldName, string fieldTypeName)
        {
            return
$$"""
namespace {{namespaceName}}
{
    public class {{className}}
    {
        public {{fieldTypeName}} {{fieldName}} { get; set; }

        public {{className}}() { }
        public {{className}}({{fieldTypeName}} value) => {{fieldName}} = value;
    }
}
""";
        }
    }
}