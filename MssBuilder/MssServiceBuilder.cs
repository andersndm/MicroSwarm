using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using Mss.Types;
using MssBuilder.Projects;
using System.Diagnostics;
using System.Text;

namespace MssBuilder
{
    public class MssServiceBuilder
    {
        private string _serviceName = "";
        private const string ENTITY_FOLDER = "Entities";

        private bool _serviceUsesValueTypes = false;

        private string GetEntityName(MssEntity entity)
        {
            if (entity.Name == "root")
            {
                return _serviceName + "Root";
            }
            else
            {
                return entity.Name;
            }
        }

        private string BuildEntityModel(MssEntity entity, IEnumerable<MssRelation> relations)
        {
            StringBuilder modelBuilder = new();
            modelBuilder.AppendLine($"            modelBuilder.Entity<{GetEntityName(entity)}>(entity =>");
            modelBuilder.AppendLine("            {");

            string entityName = GetEntityName(entity);
            string indent = "                ";

            foreach (var field in entity.Fields)
            {
                if (field.Type is MssKeyType keyType && keyType.ToString() == "PK")
                {
                    modelBuilder.AppendLine($"{indent}entity.HasKey(e => e.{field.Name});");
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

                            string toEntityName = GetEntityName(toEntity);
                            Debug.Assert(field.Type.IsPkFkPair(toField.Type));

                            if (toRel.IsMany)
                            {
                                modelBuilder.AppendLine($"{indent}entity.HasMany(e => e.{toEntityName})");
                                if (fromRel.IsMany)
                                {
                                    modelBuilder.AppendLine($"{indent}    .WithMany(o => o.{entityName})");
                                }
                                else
                                {
                                    modelBuilder.AppendLine($"{indent}    .WithOne(o => o.{entityName})");
                                }
                            }
                            else
                            {
                                modelBuilder.AppendLine($"{indent}entity.HasOne(e => e.{toEntityName})");
                                if (fromRel.IsMany)
                                {
                                    modelBuilder.AppendLine($"{indent}    .WithMany(o => o.{entityName})");
                                }
                                else
                                {
                                    modelBuilder.AppendLine($"{indent}    .WithOne(o => o.{entityName})");
                                }
                            }
                            modelBuilder.AppendLine($"{indent}    .OnDelete(DeleteBehavior.Restrict);");
                        }
                    }
                    // Cannot own a built in type
                    else if (field.Type is not MssBuiltInType && field.Type is not MssKeyType)
                    {
                        modelBuilder.AppendLine($"{indent}entity.OwnsOne(e => e.{field.Name});");
                    }
                }
            }

            modelBuilder.AppendLine("            });");
            return modelBuilder.ToString();
        }

        private MssCSharpFile BuildDbContext(MssDatabase database, string projectName, SwarmDir projectDir)
        {
            string className = projectName + "Context";
            StringBuilder contentBuilder = new();
            if (_serviceUsesValueTypes)
            {
                contentBuilder.AppendLine(UsingTemplate.Render(MssValueTypeProject.ProjectName));
            }
            contentBuilder.AppendLine(DbContextTemplate.RenderHeader(projectName, className));

            var entityNames = new List<string> { GetEntityName(database.Root) };
            entityNames.AddRange(database.Entities.Select(e => e.Name));
            contentBuilder.AppendLine(DbContextTemplate.RenderDbSets(entityNames));

            contentBuilder.AppendLine(DbContextTemplate.RenderConstructor(projectName, className));

            contentBuilder.AppendLine(DbContextTemplate.RenderOnModelCreatingHeader());

            contentBuilder.AppendLine(BuildEntityModel(database.Root, database.Relations.Where(r => r.ContainsEntity(database.Root))));
            foreach (var entity in database.Entities)
            {
                contentBuilder.AppendLine(BuildEntityModel(entity, database.Relations.Where(r => r.ContainsEntity(entity))));
            }

            contentBuilder.AppendLine(DbContextTemplate.RenderOnModelCreatingFooter());

            contentBuilder.AppendLine(DbContextTemplate.RenderFooter());

            return new(className + ".cs", projectDir, contentBuilder.ToString());
        }

        public MssWebApiProject Build(MssService service, SwarmDir solutionDir)
        {
            _serviceUsesValueTypes = false;
            _serviceName = service.Name;

            MssWebApiProject project = new(_serviceName, solutionDir);
            var entityDir = project.Dir.CreateSub(ENTITY_FOLDER);

            foreach (var entity in service.Database.Entities)
            {
                var relations = service.Database.Relations.Where(r => r.ContainsEntity(entity));
                project.AddFile(new MssEntityClassFile(entity, relations, entityDir, _serviceName));
            }

            {
                var relations = service.Database.Relations.Where(r => r.ContainsEntity(service.Database.Root));
                project.AddFile(new MssEntityClassFile(service.Database.Root, relations, entityDir, _serviceName));
            }

            project.AddFile(new MssApiProgramFile(_serviceName, project.Dir));

            if (_serviceUsesValueTypes)
            {
                project.AddProjectReference(MssValueTypeProject.ProjectName);
            }

            project.AddFile(BuildDbContext(service.Database, _serviceName, project.Dir));

            project.AddPackageReference("Microsoft.EntityFrameworkCore", "8.0.0");
            project.AddPackageReference("Microsoft.EntityFrameworkCore.Design", "8.0.0");
            // Todo: delete, this is for early testing, should use a server!!!
            project.AddPackageReference("Microsoft.EntityFrameworkCore.InMemory", "8.0.0");

            return project;
        }
    }
}