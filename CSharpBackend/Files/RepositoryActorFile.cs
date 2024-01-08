using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;
using System.Diagnostics;

namespace CSharpBackend.Files
{
    public class RepositoryActorFile : CSharpFile
    {
        public const string CLASS_NAME = "RepositoryActor";

        public RepositoryActorFile(string solutionName, MssService service, SwarmDir dir)
            : base(CLASS_NAME, dir)
        {
            Append(RepositoryActorTemplate.RenderHeader(solutionName, service.Name));

            Append(RepositoryActorTemplate.RenderSetHeader(service.Name));
            foreach (var field in service.Database.Root.Fields)
            {
                if (field.Type is MssKeyType key)
                {
                    var rels = service.Database.Relations.Where(r => r.ContainsField(field)).ToList();
                    if (rels.Count > 0)
                    {
                        Debug.Assert(rels.Count == 1);

                        var toField = rels[0].GetOppositeField(field)!;
                        var toEntity = rels[0].GetOppositeEntity(service.Database.Root)!;
                        var fromRel = rels[0].GetRelation(field)!;
                        var toRel = rels[0].GetRelation(toField)!;

                        string toEntityName = MssEntity.GetName(toEntity, service.Name);
                        Debug.Assert(field.Type.IsPkFkPair(toField.Type));

                        Append(".Include(r => r." + toEntityName + ")");
                        // Todo: include toEntity's relations if they do not point back at the root
                    }
                }
            }
            AppendLine();
            AppendLine(RepositoryActorTemplate.RenderFooter());
        }
    }
}