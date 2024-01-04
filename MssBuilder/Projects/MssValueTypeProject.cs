using MicroSwarm.FileSystem;
using Mss.Types;
using MssBuilder.Projects;

namespace MssBuilder
{
    public class MssValueTypeProject : MssClassLibraryProject
    {
        public static readonly string ProjectName = "ValueTypes";
        public MssValueTypeProject(IEnumerable<MssClassType> classes, SwarmDir solutionDir) : base(ProjectName, solutionDir)
        {
            foreach (var classType in classes)
            {
                var classFile = new MssValueClassFile(classType, Dir);
                AddFile(classFile);
            }
        }
    }
}