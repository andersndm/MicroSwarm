namespace MicroSwarm.Templates
{
    public static class UsingTemplate
    {
        public static string Render(string nameSpace)
        {
            return
$"""
using {nameSpace};
""";
        }
    }
}