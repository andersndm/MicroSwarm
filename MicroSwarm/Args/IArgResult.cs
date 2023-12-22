namespace MicroSwarm.Args
{
    public interface IArgResult
    {
        void AddStringArg(string value);
        void AddError(string error);
    }
}