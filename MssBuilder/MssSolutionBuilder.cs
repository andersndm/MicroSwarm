using MicroSwarm.FileSystem;
using Mss;
using Mss.Types;
using MssBuilder.Projects;
using System.Text;

namespace MssBuilder
{
    public static class MssSolutionBuilder
    {
        public static void GenerateServices(MssSpec spec, SwarmDir outDir, string solutionName)
        {
            Console.WriteLine("Generating solution: " + solutionName);
            SwarmDir solutionDir = outDir.CreateSub(solutionName, deleteIfExists: true);

            var solution = new MssCSharpSolution(solutionName, solutionDir);

            List<MssClassType> classes = [];
            List<MssExternType> externs = [];
            foreach (var type in spec.Types)
            {
                if (type is MssClassType classType)
                {
                    classes.Add(classType);
                }
                if (type is MssExternType externType)
                {
                    externs.Add(externType);
                }
            }

            MssValueTypeBuilder valueBuilder = new();
            MssCSharpProject valueTypeProject = valueBuilder.Build(classes, solution.Dir);
            solution.Add(valueTypeProject);

            MssServiceBuilder serviceBuilder = new();
            foreach (var service in spec.Services)
            {
                solution.Add(serviceBuilder.Build(service, solution.Dir));
            }

            solution.Write();
        }
    }
}
