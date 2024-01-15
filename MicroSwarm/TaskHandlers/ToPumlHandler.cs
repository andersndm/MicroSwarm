using CSharpBackend.Files;
using MicroSwarm.FileSystem;
using Mss;
using Mss.Types;
using System.Text;

namespace MicroSwarm.TaskHandlers
{
    public class ToPumlHandler(SwarmDir outputDir) : TaskHandlerTail<IEnumerable<MssSpec>>
    {
        private readonly SwarmDir _outputDir = outputDir;

        private static string GetServiceRef(string serviceName) => serviceName.ToLowerInvariant() + "_service";
        private static string GetDbRef(string serviceRef) => serviceRef + "_db";
        private static string GetEntityRef(string dbRef, string entityName) => dbRef + "_" + entityName.ToLowerInvariant();
        private static string GetAggregateRef(string serviceRef) => serviceRef + "_agg";

        private static void GenerateField(StringBuilder builder, MssField field, string indent)
        {
            builder.AppendLine(indent + field.Name + " : " + field.Type);
        }

        private static void GenerateRoot(StringBuilder builder, MssRoot root, string service, string serviceRef, string indent)
        {
            var rootName = AggregateClassFile.GetName(service);
            var rootRef = GetEntityRef(serviceRef, rootName);
            builder.AppendLine(indent + @$"entity ""{service}"" as {rootRef} << (R, SkyBlue) >> {{");
            foreach (var field in root.Fields)
            {
                GenerateField(builder, field, indent + "    ");
            }
            builder.AppendLine(indent + "}\n");
        }

        private static void GenerateService(StringBuilder builder, MssService service, string indent)
        {
            var serviceRef = GetServiceRef(service.Name);
            builder.AppendLine(@$"{indent}package ""{service.Name} Service"" as {serviceRef} {{");

            GenerateRoot(builder, service.Root, service.Name, serviceRef, indent + "    ");

            builder.AppendLine($"{indent}}}\n");
        }

        private static void GenerateServices(StringBuilder builder, IEnumerable<MssService> services)
        {
            builder.AppendLine(@"package ""Services"" as services <<Rectangle>> {");
            foreach (var service in services)
            {
                GenerateService(builder, service, "    ");
            }
            builder.AppendLine("}\n");
        }

        public static string Generate(string name, MssSpec spec, bool withAggRel)
        {
            var builder = new StringBuilder();

            builder.AppendLine("@startuml " + name);
            builder.AppendLine("left to right direction");
            GenerateServices(builder, spec.Services);
            builder.AppendLine("@enduml");

            return builder.ToString();
        }

        public override IResult Handle(IEnumerable<MssSpec> input)
        {
            if (!input.Any())
            {
                return IResult.BadResult("No input MssSpec supplied");
            }

            var tasks = new Task[input.Count()];
            for (int i = 0; i < input.Count(); ++i)
            {
                var spec = input.ElementAt(i);
                var name = Path.GetFileNameWithoutExtension(spec.Filename);
                tasks[i] = Task.Run(() =>
                {
                    var puml = Generate(name, spec, true);
                    var file = _outputDir.CreateFile(name + ".puml");
                    file.Write(puml);
                });
            }

            Task.WhenAll(tasks).Wait();

            return IResult.OkResult("wrote file to ...");
        }
    }
}