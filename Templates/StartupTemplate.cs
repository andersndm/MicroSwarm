namespace MicroSwarm.Templates
{
    public static class StartupTemplate
    {
        public static string RenderHeader(string serviceName, string className)
        {
            return
$$"""
namespace {{serviceName}}
{
    public class {{className}}
    {
        public {{className}}() { }
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

        public static string RenderConfigureServices()
        {
            return "public virtual void ConfigureServices(IServiceCollection services) { }";
        }

        public static string RenderConfigureHeader()
        {
            return
"""
public virtual void Configure(IApplicationBuilder app)
{
""";
        }

        public static string RenderConfigureFooter()
        {
            return
"""
}
""";
        }
    }
}