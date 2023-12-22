namespace MicroSwarm.Args
{
    public class ArgOption<T>
    {
        public readonly string? ShortForm;
        public readonly string? LongForm;
        public readonly string Description;
        public readonly Func<T, string[], int> OptionFunc;

        public ArgOption(char? shortForm, string? longForm, string description, Func<T, string[], int> func)
        {
            if (shortForm == null && longForm == null)
            {
                throw new ArgOptionException("At least one of short or long option name must be specified.");
            }
            if (shortForm != null)
            {
                ShortForm = "-" + shortForm;
            }
            if (longForm != null)
            {
                LongForm = "--" + longForm;
            }
            Description = description;
            OptionFunc = func;
        }
    }
}