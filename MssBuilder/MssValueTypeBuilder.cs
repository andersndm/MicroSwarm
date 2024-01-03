using CLI.Templates;
using Mss.Types;
using System.Diagnostics;
using MssBuilder.Projects;
using MicroSwarm.FileSystem;

namespace MssBuilder
{
    public class MssValueTypeBuilder
    {
        public static readonly string ProjectName = "ValueTypes";

        private readonly ValueClassTemplate _classTemplate = new();

        private MssCSharpFile BuildValueClassFile(MssClassType valueType, SwarmDir dir)
        {
            string filename = valueType.Name + ".cs";
            string classContent = _classTemplate.Render(ProjectName, valueType.Name, valueType.Field.Name,
                                                        valueType.Field.Type.ToString());
            MssCSharpFile result = new(filename, dir, classContent);
            return result;
        }

        public MssCSharpProject Build(IEnumerable<MssClassType> classes, SwarmDir solutionDir)
        {
            Debug.Assert(classes.Any());
            MssClassLibraryProject project = new(ProjectName, solutionDir);

            foreach (var valueType in classes)
            {
                project.AddFile(BuildValueClassFile(valueType, project.Dir));
            }

            return project;
        }
    }
}
