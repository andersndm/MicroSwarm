using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using System.Text;

namespace MssBuilder
{
    public class MssApiProgramFile : MssCSharpFile
    {
        public const string CLASS_NAME = "Program";

        public MssApiProgramFile(string serviceName, string startupName, SwarmDir dir) : base(CLASS_NAME, dir)
        {
            AppendLine(ProgramTemplate.RenderHeader(serviceName, CLASS_NAME));
            Indentation = CLASS_MEMBER_INDENT;

            // Todo: properly implement templates etc once real endpoints are added
            AppendLine(@"public const string ROOT = ""/"";");
            AppendLine(@"public const string QUERY = ""query"";");
            AppendLine(@"public const string CMD = ""cmd"";");
            AppendLine();

            AppendLine(ProgramTemplate.RenderHostBuilderMethod(startupName));
            AppendLine();

            AppendLine(ProgramTemplate.RenderMainMethod());

            ClearIndentation();
            AppendLine(ProgramTemplate.RenderFooter());
        }
    }
}