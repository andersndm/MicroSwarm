using MicroSwarm.FileSystem;
using Mss.Types;
using CSharpBackend.Projects;
using CSharpBackend.Files;

namespace CSharpBackend
{
    public class ValueTypeProject : ClassLibraryProject
    {
        public static readonly string ProjectName = "ValueTypes";
        public ValueTypeProject(IEnumerable<MssClassType> classes, SwarmDir solutionDir) : base(ProjectName, solutionDir)
        {
            foreach (var classType in classes)
            {
                var classFile = new ValueClassFile(classType, Dir);
                AddFile(classFile);
            }
        }
    }
}