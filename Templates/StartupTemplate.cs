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

        public static string RenderConfigureServices(string serviceName)
        {
            return
$$"""
public virtual void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}
""";
        }

        public static string RenderConfigure()
        {
            return
"""
public virtual void Configure(IApplicationBuilder app)
{
    // Add Endpoints
    app.UseRouting();
    app.UseHttpsRedirection();
    app.UseEndpoints(e =>
    {
        e.MapControllers();
    });

    // Add Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}
""";
        }
    }
}