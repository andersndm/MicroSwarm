using MicroSwarm.FileSystem;
using Mss;
using Mss.Types;
using MssBuilder.Projects;

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

            solution.Add(new MssValueTypeProject(classes, solutionDir));

            foreach (var service in spec.Services)
            {
                solution.Add(new MssWebApiProject(solutionDir, service));
            }

            solution.Write();
        }
    }
}
