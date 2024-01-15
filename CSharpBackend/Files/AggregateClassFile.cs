using CSharpBackend.Projects;
using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;

namespace CSharpBackend.Files
{
    public class AggregateClassFile : CSharpFile
    {
        private static readonly string _suffix = "Aggregate";

        public AggregateClassFile(string solutionName, string serviceName, SwarmDir dir, IEnumerable<MssField> fields)
            : base(GetName(serviceName), dir)
        {
            string name = GetName(serviceName);

            AppendLine(AggregateTemplate.RenderHeader(solutionName, name));
            Indentation = CLASS_MEMBER_INDENT;

            foreach (var field in fields)
            {
                AppendLine(PropertyTemplate.Render(field.Name, field.Type.ToCSharp()));
            }

            ClearIndentation();
            Append(AggregateTemplate.RenderFooter());
        }

        public static string GetName(string serviceName)
        {
            return serviceName + _suffix;
        }
    }
}