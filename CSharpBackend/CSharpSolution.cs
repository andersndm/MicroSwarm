using MicroSwarm.FileSystem;
using Mss;
using Mss.Types;
using CSharpBackend.Projects;
using System.Text;

namespace CSharpBackend
{
    public class CSharpSolution
    {
        public string Name { get; }
        public SwarmDir Dir { get => _dir; }

        private readonly SwarmDir _dir;

        private readonly List<CSharpProject> projects = [];

        protected readonly string _formatVersionMajor = "12";
        protected readonly string _formatVersionMinor = "00";

        protected readonly string _vsVersionMajor = "17";
        protected readonly string _vsVersionMinor = "0";
        protected readonly string _vsVersionPatch = "31903";
        protected readonly string _vsVersionSubPatch = "59";

        protected readonly string _vsMinVersionMajor = "10";
        protected readonly string _vsMinVersionMinor = "0";
        protected readonly string _vsMinVersionPatch = "40291";
        protected readonly string _vsMinVersionSubPatch = "1";

        private readonly StringBuilder _builder = new();
        private int _indent = 0;
        private readonly string _singleIndentation = "    ";

        public CSharpSolution(string name, SwarmDir dir, MssSpec spec)
        {
            Name = name;

            _dir = dir.CreateSub(Name, deleteIfExists: true);

            List<MssClassType> classes = [];
            List<MssExternType> externs = [];
            foreach (var type in spec.Types)
            {
                if (type is MssClassType classType)
                {
                    classes.Add(classType);
                }
                if (type is MssExternType externType)
                {
                    externs.Add(externType);
                }
            }

            Add(new ValueTypeProject(classes, Dir));
            Add(new CoreProject(Name, spec, _dir));

            foreach (var service in spec.Services)
            {
                Add(new WebApiProject(service, Dir));
            }
        }

        private void Indent() => ++_indent;
        private void UnIndent()
        {
            if (_indent > 0)
            {
                --_indent;
            }
        }

        private void ApplyIndentation()
        {
            for (int i = 0; i < _indent; i++)
            {
                _builder.Append(_singleIndentation);
            }
        }

        private void AppendLine(string line)
        {
            var lines = line.Split(Environment.NewLine);
            foreach (var newline in lines)
            {
                ApplyIndentation();
                _builder.AppendLine(newline);
            }
        }

        public void Add(CSharpProject project)
        {
            projects.Add(project);
        }

        private void WriteGlobalProperties()
        {
            AppendLine($"Microsoft Visual Studio Solution File, Format Version {_formatVersionMajor}.{_formatVersionMinor}");
            AppendLine($"# Visual Studio Version {_vsVersionMajor}");
            AppendLine($"VisualStudioVersion = {_vsVersionMajor}.{_vsVersionMinor}.{_vsVersionPatch}.{_vsVersionSubPatch}");
            AppendLine($"MinimumVisualStudioVersion = {_vsMinVersionMajor}.{_vsMinVersionMinor}.{_vsMinVersionPatch}.{_vsMinVersionSubPatch}");
        }

        private void WriteProjectSection()
        {
            foreach (var project in projects)
            {
                AppendLine(@$"Project(""{{{project.TypeGuid}}}"") = ""{project.Name}"", ""{project.Name}\{project.Name}.csproj"", ""{{{project.Id}}}""");
                AppendLine("EndProject");
            }
        }

        private void WriteGlobalSection()
        {
            AppendLine("Global");
            Indent();
            AppendLine("GlobalSection(SolutionConfigurationPlatforms) = preSolution");
            Indent();
            AppendLine("Debug|Any CPU = Debug|Any CPU");
            AppendLine("Release|Any CPU = Release|Any CPU");
            UnIndent();
            AppendLine("EndGlobalSection");
            AppendLine("GlobalSection(ProjectConfigurationPlatforms) = postSolution");
            Indent();
            foreach (var project in projects)
            {
                AppendLine($"{{{project.Id}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                AppendLine($"{{{project.Id}}}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                AppendLine($"{{{project.Id}}}.Release|Any CPU.ActiveCfg = Release|Any CPU");
                AppendLine($"{{{project.Id}}}.Release|Any CPU.Build.0 = Release|Any CPU");
            }
            UnIndent();
            AppendLine("EndGlobalSection");
            AppendLine("GlobalSection(SolutionProperties) = preSolution");
            Indent();
            AppendLine("HideSolutionNode = FALSE");
            UnIndent();
            AppendLine("EndGlobalSection");
            AppendLine("GlobalSection(ExstensibilityGlobals) = postSolution");
            Indent();
            AppendLine($"SolutionGuid = {{{Guid.NewGuid()}}}");
            UnIndent();
            AppendLine("EndGlobalSection");
            UnIndent();
            AppendLine("EndGlobal");
        }

        public void Write()
        {
            WriteGlobalProperties();
            WriteProjectSection();
            WriteGlobalSection();

            _dir.CreateFile(Name + ".sln").Write(_builder.ToString());

            foreach (var project in projects)
            {
                project.Write();
            }

        }
    }
}