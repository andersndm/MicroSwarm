using HandlebarsDotNet;

namespace CLI.Templates
{
    public abstract class HandlebarTemplate(string templateStr)
    {
        protected HandlebarsTemplate<object, object> _template = Handlebars.Compile(templateStr);
    }
}