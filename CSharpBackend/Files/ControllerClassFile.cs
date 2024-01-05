using MicroSwarm.FileSystem;
using MicroSwarm.Templates;

namespace CSharpBackend.Files
{
    public class ControllerClassFile : CSharpFile
    {
        public const string CLASS_NAME_SUFFIX = "Controller";

        public ControllerClassFile(string serviceName, SwarmDir dir)
            : base(serviceName + CLASS_NAME_SUFFIX, dir)
        {
            AppendLine(ControllerTemplate.Render(serviceName, serviceName + CLASS_NAME_SUFFIX));
        }
    }
}