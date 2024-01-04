using MicroSwarm.FileSystem;
using MicroSwarm.Templates;

namespace MssBuilder
{
    public class StartupClassFile : MssCSharpFile
    {
        public const string CLASS_NAME = "Startup";

        public StartupClassFile(string serviceName, SwarmDir dir) : base(CLASS_NAME, dir)
        {
            AppendLine(StartupTemplate.RenderHeader(serviceName, CLASS_NAME));
            AppendLine();

            Indentation = CLASS_MEMBER_INDENT;
            AppendLine(StartupTemplate.RenderConfigureServices());
            AppendLine();

            AppendLine(StartupTemplate.RenderConfigureHeader());
            // Todo: properly implement templates etc once real endpoints are added
            Indent();
            AddEndpoint("Program.ROOT + Program.QUERY", serviceName + " does not have query functionality yet!", 400);
            AddEndpoint("Program.ROOT + Program.CMD", serviceName + " does not have cmd functionality yet!", 400);
            UnIndent();

            AppendLine(StartupTemplate.RenderConfigureFooter());
            ClearIndentation();
            AppendLine(StartupTemplate.RenderFooter());
        }

        private void AddEndpoint(string endpoint, string description, int statusCode)
        {
            AppendLine("app.Map(" + endpoint + ", builder => builder.Run(async context =>");
            AppendLine("{");
            Indent();
            AppendLine("context.Response.StatusCode = " + statusCode + ";");
            AppendLine("await context.Response.WriteAsync(\"" + description + "\");");
            UnIndent();
            AppendLine("}));");
        }
    }
}