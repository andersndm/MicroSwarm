namespace CLI.Templates
{
    public class UsingTemplate() : HandlebarTemplate(TEMPLATE_STR)
    {
        private const string TEMPLATE_STR = "using {{Namespace}};\n";

        public string Render(string nameSpace)
        {
            var data = new
            {
                Namespace = nameSpace,
            };
            return _template(data);
        }
    }
}