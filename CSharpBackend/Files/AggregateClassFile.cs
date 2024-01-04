using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;

namespace CSharpBackend.Files
{
    public class AggregateClassFile : CSharpFile
    {
        private static readonly string _suffix = "Aggregate";

        public bool UsesValueTypes { get => _usesValueTypes; }
        private readonly bool _usesValueTypes = false;

        public AggregateClassFile(string serviceName, SwarmDir dir, IEnumerable<MssAggregateField> fields)
            : base(GetName(serviceName), dir)
        {
            string name = GetName(serviceName);

            AppendLine(EntityTemplate.RenderHeader(serviceName, name));
            Indentation = CLASS_MEMBER_INDENT;

            foreach (var field in fields)
            {
                if (field.Field.Type is MssClassType)
                {
                    _usesValueTypes = true;
                }
                AppendLine(PropertyTemplate.Render(field.Field.Name, field.Field.Type.ToCSharp()));
            }

            ClearIndentation();
            Append(EntityTemplate.RenderFooter());

            if (_usesValueTypes)
            {
                PrependLine(UsingTemplate.Render(ValueTypeProject.ProjectName) + "\n");
            }
        }

        public static string GetName(string serviceName)
        {
            return serviceName + _suffix;
        }
    }
}