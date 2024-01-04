namespace MicroSwarm.Templates
{
    public static class ProgramTemplate
    {
        public static string RenderHeader(string serviceName, string className)
        {
            return
$$"""
namespace {{serviceName}}
{
    internal sealed class Program
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

        public static string RenderHostBuilderMethod(string startupName)
        {
            return
$$"""
private static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<{{startupName}}>();
        });
}
""";
        }

        public static string RenderMainMethod()
        {
            return
"""
public static void Main(string[] args)
{
    var app = CreateHostBuilder(args).Build();
    app.Run();
}
""";
        }
    }
}