using CSharpBackend.Files;
using MicroSwarm.FileSystem;
using System.Xml.Linq;

namespace CSharpBackend.Projects
{
    public class ProjectDependency(string name)
    {
        public readonly string Name = name;
        public readonly XElement Xml = new("ProjectReference",
                                           new XAttribute("Include", $"..\\{name}\\{name}.csproj"));
    }

    public abstract class CSharpProject(string name, SwarmDir solutionDir)
    {
        // solution file uuid for generic c# project/class library
        protected Guid _csharpProjectUUID = new("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC");
        protected Guid _aspCoreProjectUUID = new("9A19103F-16F7-4668-BE54-9A1E7A4F7556");

        public readonly string Name = name;
        public readonly Guid Id = Guid.NewGuid();

        public SwarmDir Dir { get => _dir; }
        private readonly SwarmDir _dir = solutionDir.CreateSub(name, deleteIfExists: true);
        protected bool _useWebSdk = false;

        public virtual Guid TypeGuid { get => _csharpProjectUUID; }

        public IEnumerable<string> Dependencies { get => _projectReferences.Select(d => d.Name); }

        private readonly List<CSharpFile> _files = [];

        protected readonly List<ProjectDependency> _projectReferences = [];
        protected readonly List<XElement> _packageReferences = [];

        protected readonly int _dotnet_major_version = 8;
        protected readonly int _dotnet_minor_version = 0;

        public void AddFile(CSharpFile file) => _files.Add(file);
        public void AddFiles(IEnumerable<CSharpFile> file) => _files.AddRange(file);

        public void AddPackageReference(string packageName, string packageVersion)
        {
            _packageReferences.Add(new XElement("PackageReference",
                                                new XAttribute("Include", packageName),
                                                new XAttribute("Version", packageVersion)));
        }

        protected XElement CreatePackageReferences()
        {
            var result = new XElement("ItemGroup");
            foreach (var reference in _packageReferences)
            {
                result.Add(reference);
            }
            return result;
        }

        public void AddProjectReference(string projectName)
        {
            _projectReferences.Add(new ProjectDependency(projectName));
        }

        protected XElement CreateProjectReferences()
        {
            var result = new XElement("ItemGroup");
            foreach (var reference in _projectReferences)
            {
                result.Add(reference.Xml);
            }
            return result;
        }

        protected XElement CreateProjectHeader()
        {
            if (_useWebSdk)
            {
                return new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk.Web"));
            }
            else
            {
                return new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"));
            }
        }

        protected XElement CreatePropertyGroup()
        {
            return new XElement("PropertyGroup",
                                new XElement("TargetFramework", $"net{_dotnet_major_version}.{_dotnet_minor_version}"),
                                new XElement("ImplicitUsings", "enable"),
                                new XElement("Nullable", "enable"));
        }

        protected abstract string CreateProjectFile();

        public void Write()
        {
            var projectFile = _dir.CreateFile(Name + ".csproj");
            projectFile.Write(CreateProjectFile());

            foreach (var file in _files)
            {
                file.Write();
            }
        }
    }
}