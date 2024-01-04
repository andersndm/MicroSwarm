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

        public MssWebApiProject Build(MssService service, SwarmDir solutionDir)
        {
            bool serviceUsesValueTypes = false;
            _serviceName = service.Name;

            MssWebApiProject project = new(_serviceName, solutionDir);
            var entityDir = project.Dir.CreateSub(ENTITY_FOLDER);

            foreach (var entity in service.Database.Entities)
            {
                var relations = service.Database.Relations.Where(r => r.ContainsEntity(entity));
                var entityClass = new MssEntityClassFile(entity, relations, entityDir, _serviceName);
                serviceUsesValueTypes |= entityClass.UsesValueTypes;
                project.AddFile(entityClass);
            }

            {
                var relations = service.Database.Relations.Where(r => r.ContainsEntity(service.Database.Root));
                project.AddFile(new MssEntityClassFile(service.Database.Root, relations, entityDir, _serviceName));
            }

            project.AddFile(new MssApiProgramFile(_serviceName, project.Dir));

            if (serviceUsesValueTypes)
            {
                project.AddProjectReference(MssValueTypeProject.ProjectName);
            }

            project.AddFile(new MssDbContextClassFile(service.Database, _serviceName, project.Dir, serviceUsesValueTypes));

            project.AddPackageReference("Microsoft.EntityFrameworkCore", "8.0.0");
            project.AddPackageReference("Microsoft.EntityFrameworkCore.Design", "8.0.0");
            // Todo: delete, this is for early testing, should use a server!!!
            project.AddPackageReference("Microsoft.EntityFrameworkCore.InMemory", "8.0.0");

            return project;
        }
    }
}