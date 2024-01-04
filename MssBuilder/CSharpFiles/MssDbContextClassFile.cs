using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;
using System.Diagnostics;

namespace MssBuilder
{
    public class MssDbContextClassFile : MssCSharpFile
    {
        private static readonly string _classNameSuffix = "DbContext";
        private readonly string _serviceName;

        private void AddEntityModel(MssEntity entity, IEnumerable<MssRelation> relations)
        {
            string entityName = MssEntity.GetName(entity, _serviceName);
            AppendLine(DbContextTemplate.RenderEntitySpecHeader(entityName));

            Indent();
            foreach (var field in entity.Fields)
            {
                if (field.Type is MssKeyType keyType && keyType.ToString() == "PK")
                {
                    AppendLine(DbContextTemplate.RenderEntityPrimaryKey(field.Name));
                }
                else
                {
                    var rels = relations.Where(r => r.ContainsField(field));
                    if (rels.Any())
                    {
                        foreach (var rel in rels)
                        {
                            var toField = rel.GetOppositeField(field)!;
                            var toEntity = rel.GetOppositeEntity(entity)!;
                            var fromRel = rel.GetRelation(field)!;
                            var toRel = rel.GetRelation(toField)!;

                            string toEntityName = MssEntity.GetName(toEntity, _serviceName);
                            Debug.Assert(field.Type.IsPkFkPair(toField.Type));

                            if (toRel.IsMany)
                            {
                                if (fromRel.IsMany)
                                {
                                    AppendLine(DbContextTemplate.RenderEntityManyToMany(entityName, toEntityName));
                                }
                                else
                                {
                                    AppendLine(DbContextTemplate.RenderEntityManyToOne(entityName, toEntityName));
                                }
                            }
                            else
                            {
                                if (fromRel.IsMany)
                                {
                                    AppendLine(DbContextTemplate.RenderEntityOneToMany(entityName, toEntityName));
                                }
                                else
                                {
                                    AppendLine(DbContextTemplate.RenderEntityOneToOne(entityName, toEntityName));
                                }
                            }
                        }
                    }
                    // Cannot own a built in type
                    else if (field.Type is not MssBuiltInType && field.Type is not MssKeyType)
                    {
                        AppendLine(DbContextTemplate.RenderEntityOwns(field.Name));
                    }
                }
            }
            UnIndent();
            AppendLine(DbContextTemplate.RenderEntitySpecFooter());
        }

        public MssDbContextClassFile(MssDatabase database, string serviceName, SwarmDir dir, bool usesValueTypes)
            : base(serviceName + _classNameSuffix, dir)
        {
            _serviceName = serviceName;
            string className = _serviceName + _classNameSuffix;

            if (usesValueTypes)
            {
                AppendLine(UsingTemplate.Render(MssValueTypeProject.ProjectName));
            }
            AppendLine(DbContextTemplate.RenderHeader(_serviceName, className));

            var entityNames = new List<string> { MssEntity.GetName(database.Root, _serviceName) };
            entityNames.AddRange(database.Entities.Select(e => e.Name));
            AppendLine(DbContextTemplate.RenderDbSets(entityNames));

            AppendLine(DbContextTemplate.RenderConstructor(_serviceName, className));
            AppendLine(DbContextTemplate.RenderOnModelCreatingHeader());

            const int METHOD_CONTENT_INDENT = 3;
            Indent(METHOD_CONTENT_INDENT);

            AddEntityModel(database.Root, database.Relations.Where(r => r.ContainsEntity(database.Root)));
            foreach (var entity in database.Entities)
            {
                AddEntityModel(entity, database.Relations.Where(r => r.ContainsEntity(entity)));
            }

            ClearIndentation();

            AppendLine(DbContextTemplate.RenderOnModelCreatingFooter());
            AppendLine(DbContextTemplate.RenderFooter());
        }
    }
}