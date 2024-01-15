using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;

namespace CSharpBackend.Files
{
    public class DbContextClassFile : CSharpFile
    {
        private static readonly string _classNameSuffix = "DbContext";
        private readonly string _serviceName;
        private readonly string _rootName;

        public DbContextClassFile(MssRoot root, string solutionName, string serviceName, SwarmDir dir)
            : base(serviceName + _classNameSuffix, dir)
        {
            _serviceName = serviceName;
            _rootName = AggregateClassFile.GetName(_serviceName);
            string className = _serviceName + _classNameSuffix;

            AppendLine(DbContextTemplate.RenderHeader(solutionName, _serviceName, className));

            AppendLine(DbContextTemplate.RenderDbSets([_rootName]));

            AppendLine(DbContextTemplate.RenderConstructor(_serviceName, className));
            AppendLine();

            AppendLine(DbContextTemplate.RenderOnModelCreatingHeader());

            Indentation = METHOD_CONTENT_INDENT;

            AddRoot(root);

            ClearIndentation();

            AppendLine(DbContextTemplate.RenderOnModelCreatingFooter());
            AppendLine(DbContextTemplate.RenderFooter());
        }

        private void AddRoot(MssRoot root)
        {
            AppendLine(DbContextTemplate.RenderEntitySpecHeader(_rootName));

            Indent();
            // add Id field
            AppendLine(DbContextTemplate.RenderEntityPrimaryKey("Id"));
            foreach (var field in root.Fields)
            {
                if (field.Type is MssListType)
                {
                    AppendLine(DbContextTemplate.RenderEntityOwns(field.Name));
                }
            }
            UnIndent();
            AppendLine(DbContextTemplate.RenderEntitySpecFooter());
        }
    }
}