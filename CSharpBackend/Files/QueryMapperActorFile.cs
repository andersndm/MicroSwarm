using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;
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

            foreach (var aggField in service.AggregateFields)
            {
                Indentation = METHOD_CONTENT_INDENT;
                Indent(4);
                Append(aggField.Field.Name + " = ");
                ClearIndentation();
                if (aggField.Mapping is MssAggregateFieldMappingDirect direct)
                {
                    Append("entity.");
                    if (direct.Mapping.Count == 1)
                    {
                        AddFieldAccess(direct.Mapping[0].Item2);
                    }
                    else
                    {
                        Debug.Assert(direct.Mapping.Count == 2);
                        Append(direct.Mapping[1].Item1.Name + ".");
                        AddFieldAccess(direct.Mapping[1].Item2);
                    }
                }
                else
                {
                    var where = (MssAggregateFieldMappingWhere)aggField.Mapping ??
                        throw new InvalidCastException("Expected MssAggregateFieldMappingWhere");
                    Append("entity.");
                    if (where.Mapping.Count == 1)
                    {
                        Append(where.Mapping[0].Item1.Name);
                        Append(".Select(a => a.");
                        Append(where.Mapping[0].Item2.Name);
                        AppendLine(").ToList(),");
                    }
                    else
                    {
                        // Todo: will this ever be relevant?
                        throw new NotImplementedException();
                    }

                }
            }

            AppendLine(QueryMapperActorTemplate.RenderFooter());
        }

        private void AddFieldAccess(MssField field)
        {
            Append(field.Name);
            if (field.Type is MssClassType classType)
            {
                Append("." + classType.Field.Name);
            }
            AppendLine(",");
        }
    }
}