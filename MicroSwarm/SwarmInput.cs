using MicroSwarm.Args;

namespace MicroSwarm
{
    public class SwarmInput : IArgResult
    {
        public bool VersionRequested { get; set; } = false;
        public bool HelpRequested { get; set; } = false;
        public bool OutputPlantUml { get; set; } = false;
        public bool ToPuml { get; set; } = false;

        public List<string> Files { get; set; } = [];
        public string? OutputDir { get; set; } = null;

        public List<string> Errors = [];

        public void AddStringArg(string value)
        {
            Files.Add(value);
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}