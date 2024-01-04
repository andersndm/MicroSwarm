using MicroSwarm.FileSystem;
using System.Xml.Linq;

namespace CSharpBackend.Projects
{
    public class ClassLibraryProject(string name, SwarmDir solutionDir) : CSharpProject(name, solutionDir)
    {
        protected override string CreateProjectFile()
        {
            XElement header = CreateProjectHeader();
            header.Add(CreatePropertyGroup());

            if (_packageReferences.Count > 0)
            {
                header.Add(CreatePackageReferences());
            }

            if (_projectReferences.Count > 0)
            {
                header.Add(CreateProjectReferences());
            }

            return header.ToString();
        }
    }
}