using MicroSwarm.Templates;
using MicroSwarm.FileSystem;
using Mss.Types;

namespace CSharpBackend.Files
{
    public class ValueClassFile : CSharpFile
    {
        public static readonly string ProjectName = "ValueTypes";

        public ValueClassFile(MssClassType valueType, SwarmDir dir) : base(valueType.Name, dir)
        {
            string classContent = ValueClassTemplate.Render(ProjectName, valueType.Name, valueType.Field.Name,
                                                            valueType.Field.Type.ToString());
            Append(classContent);
        }
    }
}