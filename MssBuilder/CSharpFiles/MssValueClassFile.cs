using MicroSwarm.Templates;
using MicroSwarm.FileSystem;
using Mss.Types;

namespace MssBuilder
{
    public class MssValueClassFile : MssCSharpFile
    {
        public static readonly string ProjectName = "ValueTypes";

        public MssValueClassFile(MssClassType valueType, SwarmDir dir) : base(valueType.Name, dir)
        {
            string classContent = ValueClassTemplate.Render(ProjectName, valueType.Name, valueType.Field.Name,
                                                            valueType.Field.Type.ToString());
            Append(classContent);
        }
    }
}