using CSharpBackend.Files;
using MicroSwarm.FileSystem;
using MicroSwarm.Templates;
using Mss;
using System.Xml.Linq;

namespace CSharpBackend.Projects
{
    public class WebApiProject : CSharpProject
    {
        public override Guid TypeGuid { get => _aspCoreProjectUUID; }

        public WebApiProject(MssService service, SwarmDir solutionDir) : base(service.Name, solutionDir)
        {
            _useWebSdk = true;

            AddFile(new DbContextClassFile(service.Root, solutionDir.Name, service.Name, Dir));
            AddFile(new ControllerClassFile(solutionDir.Name, service.Name, Dir));
            AddFile(new StartupClassFile(solutionDir.Name, service.Name, Dir));
            AddFile(new ApiProgramFile(service.Name, StartupClassFile.CLASS_NAME, Dir));

            AddFile(new QueryRootFilterFile(solutionDir.Name, service.Name, Dir));

            var actorDir = Dir.CreateSub("Actors");
            AddFile(new CSharpFile(service.Name + "ControllerActor", actorDir,
                                   ControllerActorTemplate.Render(solutionDir.Name, service.Name)));
            AddFile(new RepositoryActorFile(solutionDir.Name, service, actorDir));

            var cmdActorDir = actorDir.CreateSub("Cmd");
            AddFile(new CSharpFile("CmdActor", cmdActorDir,
                                   CmdActorTemplate.Render(solutionDir.Name, service.Name)));

            var queryActorDir = actorDir.CreateSub("Query");
            AddFile(new CSharpFile("QueryActor", queryActorDir,
                                   QueryActorTemplate.Render(solutionDir.Name, service.Name)));
            AddFile(new CSharpFile("QueryFilterCreatorActor", queryActorDir,
                                   QueryFilterCreatorActorTemplate.Render(solutionDir.Name, service.Name)));
            AddFile(new CSharpFile("QuerySerializeActor", queryActorDir,
                                    QuerySerializeActorTemplate.Render(solutionDir.Name, service.Name)));
            AddFile(new QueryMapperActorFile(solutionDir.Name, service, queryActorDir));

            AddProjectReference(solutionDir.Name + CoreProject.Suffix);

            // add swagger
            AddPackageReference("Microsoft.AspNetCore.OpenApi", "8.0.0");
            AddPackageReference("Swashbuckle.AspNetCore.SwaggerGen", "6.5.0");
            AddPackageReference("Swashbuckle.AspNetCore.SwaggerUI", "6.5.0");

            // add EF
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