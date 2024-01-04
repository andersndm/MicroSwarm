using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;
using System.Diagnostics;

namespace CSharpBackend.Files
{
    public class AggregateClassFile : CSharpFile
    {
        private static readonly string _suffix = "Aggregate";

        public AggregateClassFile(string serviceName, SwarmDir dir, IEnumerable<MssAggregateField> fields)
            : base(GetName(serviceName), dir)
        {
            string name = GetName(serviceName);

            AppendLine(EntityTemplate.RenderHeader(serviceName, name));
            Indentation = CLASS_MEMBER_INDENT;

            foreach (var field in fields)
            {
                Debug.Assert(field.Field.Type is MssBuiltInType);
                AppendLine(PropertyTemplate.Render(field.Field.Name, field.Field.ToString()));
            }

            ClearIndentation();
            Append(EntityTemplate.RenderFooter());
        }

        public static string GetName(string serviceName)
        {
            return serviceName + _suffix;
        }
    }
}