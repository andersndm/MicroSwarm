using MicroSwarm.FileSystem;
using System.Xml.Linq;

namespace MssBuilder.Projects
{
    public class MssWebApiProject : MssCSharpProject
    {
        public override Guid TypeGuid { get => _aspCoreProjectUUID; }

        public MssWebApiProject(string name, SwarmDir solutionDir) : base(name, solutionDir)
        {
            _useWebSdk = true;
        }

        protected override string CreateProjectFile()
        {
            XElement header = CreateProjectHeader();
            var propertyGrp = CreatePropertyGroup();
            propertyGrp.Add(new XElement("OutputType", "Exe"));
            header.Add(propertyGrp);

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