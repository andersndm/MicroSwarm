using CSharpBackend.Files;
using MicroSwarm.FileSystem;
using Mss;
using System.Xml.Linq;

namespace CSharpBackend.Projects
{
    public class WebApiProject : CSharpProject
    {
        public override Guid TypeGuid { get => _aspCoreProjectUUID; }
        private const string ENTITY_FOLDER = "Entities";

        public WebApiProject(MssService service, SwarmDir solutionDir) : base(service.Name, solutionDir)
        {
            _useWebSdk = true;

            bool serviceUsesValueTypes = false;

            var entityDir = Dir.CreateSub(ENTITY_FOLDER);

            foreach (var entity in service.Database.Entities)
            {
                var relations = service.Database.Relations.Where(r => r.ContainsEntity(entity));
                var entityClass = new EntityClassFile(entity, relations, entityDir, service.Name);
                serviceUsesValueTypes |= entityClass.UsesValueTypes;
                AddFile(entityClass);
            }

            {
                var relations = service.Database.Relations.Where(r => r.ContainsEntity(service.Database.Root));
                var rootClass = new EntityClassFile(service.Database.Root, relations, entityDir, service.Name);
                serviceUsesValueTypes |= rootClass.UsesValueTypes;
                AddFile(rootClass);
            }

            AddFile(new DbContextClassFile(service.Database, service.Name, Dir, serviceUsesValueTypes));
            var aggClass = new AggregateClassFile(service.Name, Dir, service.AggregateFields);
            serviceUsesValueTypes |= aggClass.UsesValueTypes;
            AddFile(aggClass);

            AddFile(new StartupClassFile(service.Name, Dir));
            AddFile(new ApiProgramFile(service.Name, StartupClassFile.CLASS_NAME, Dir));

            if (serviceUsesValueTypes)
            {
                AddProjectReference(ValueTypeProject.ProjectName);
            }

            AddPackageReference("Microsoft.EntityFrameworkCore", "8.0.0");
            AddPackageReference("Microsoft.EntityFrameworkCore.Design", "8.0.0");
            // Todo: delete, this is for early testing, should use a server!!!
            AddPackageReference("Microsoft.EntityFrameworkCore.InMemory", "8.0.0");
        }

        protected override string CreateProjectFile()
        {
            XElement header = CreateProjectHeader();
            var propertyGrp = CreatePropertyGroup();
            propertyGrp.Add(new XElement("OutputType", "Exe"));
            header.Add(propertyGrp);

            if (_packageReferences.Count > 0)
            {
                header.Add(CreatePackageReferences());
            }

            if (_projectReferences.Count > 0)
            {
                header.Add(CreateProjectReferences());
            }

            return header.ToString();
        }
    }
}