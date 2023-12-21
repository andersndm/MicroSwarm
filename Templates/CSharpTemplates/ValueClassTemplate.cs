namespace CLI.Templates
{
    public class ValueClassTemplate() : HandlebarTemplate(TEMPLATE_STR)
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

        public string Render(string namespaceName, string className, string fieldName, string fieldTypeName)
        {
            var data = new
            {
                Namespace = namespaceName,
                ClassName = className,
                TypeName = fieldTypeName,
                PropName = fieldName
            };
            return _template(data);
        }
    }
}