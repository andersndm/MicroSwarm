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
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        public {{fieldTypeName}} {{fieldName}} { get; set; }

        public {{className}}() { }
        public {{className}}({{fieldTypeName}} value) => {{fieldName}} = value;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    }
}
""";
        }
    }
}