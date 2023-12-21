namespace CLI.Templates
{
    public class DbContextHeaderTemplate() : HandlebarTemplate(TEMPLATE_STR)
    {
        private const string TEMPLATE_STR =
@"using {{Service}}.Entities;
using Microsoft.EntityFrameworkCore;

namespace {{Service}}
{
    public class {{Class}} : DbContext
    {
";

        public string Render(string serviceName, string className)
        {
            var data = new
            {
                Service = serviceName,
                Class = className
            };
            return _template(data);
        }
    }
}