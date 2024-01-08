using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using System.Diagnostics;

namespace CSharpBackend.Files
{
    public class QueryMapperActorFile : CSharpFile
    {
        public const string CLASS_NAME = "QueryMapperActor";
        public QueryMapperActorFile(string solutionName, MssService service, SwarmDir dir)
            : base(CLASS_NAME, dir)
        {
            AppendLine(QueryMapperActorTemplate.RenderHeader(solutionName, service.Name));
            Indentation = METHOD_CONTENT_INDENT;
            Indent(4);

            foreach (var aggField in service.AggregateFields)
            {
                if (aggField.Mapping is MssAggregateFieldMappingDirect direct)
                {
                    Append(aggField.Field.Name + " = ");
                    Append("entity.");
                    if (direct.Mapping.Count == 1)
                    {
                        AppendLine(direct.Mapping[0].Item2.Name + ",");
                    }
                    else
                    {
                        Debug.Assert(direct.Mapping.Count == 2);
                        Append(direct.Mapping[1].Item1.Name + ".");
                        Append(direct.Mapping[1].Item2.Name + ",");
                    }
                }
                else
                {
                    // TODO: fill in for other mappings, handling to many
                    //throw new NotImplementedException();
                }
            }

            ClearIndentation();
            AppendLine(QueryMapperActorTemplate.RenderFooter());
        }
    }
}