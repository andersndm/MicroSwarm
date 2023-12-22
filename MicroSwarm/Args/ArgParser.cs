namespace MicroSwarm.Args
{
    public class ArgParser<T>
        where T : IArgResult, new()
    {
        private readonly Dictionary<string, ArgOption<T>> _options = [];

        public void AddOption(ArgOption<T> option)
        {
            if (option.ShortForm != null)
            {
                _options.Add(option.ShortForm, option);
            }
            if (option.LongForm != null)
            {
                _options.Add(option.LongForm, option);
            }
        }

        public T Parse(string[] args)
        {
            T result = new();
            for (int argIndex = 0; argIndex < args.Length; argIndex++)
            {
                string arg = args[argIndex];
                if (arg.StartsWith('-'))
                {
                    if (_options.TryGetValue(arg, out var option))
                    {
                        argIndex += option.OptionFunc(result, new ArraySegment<string>(args, argIndex, args.Length - argIndex).ToArray());
                    }
                    else
                    {
                        result.AddError("Unknown option: '" + arg + "'.");
                    }
                }
                else
                {
                    result.AddStringArg(arg);
                }
            }
            return result;
        }

        public void PrintOptions()
        {
            if (_options.Count != 0)
            {
                Console.WriteLine("options:");
                var options = _options.Values.Distinct();
                int longestLongForm = 0;
                foreach (var option in options)
                {
                    if (option.LongForm != null && option.LongForm.Length > longestLongForm)
                    {
                        longestLongForm = option.LongForm.Length;
                    }
                }

                foreach (var option in options)
                {
                    if (option.ShortForm != null)
                    {
                        Console.Write("    " + option.ShortForm);
                    }
                    else
                    {
                        Console.Write("      ");
                    }

                    Console.Write(" | ");
                    if (option.LongForm != null)
                    {
                        string spaces = new(' ', longestLongForm - option.LongForm.Length);
                        Console.Write(option.LongForm + spaces);
                    }
                    else
                    {
                        Console.Write(new string(' ', longestLongForm));
                    }

                    Console.WriteLine(" | " + option.Description);
                }
            }
        }
    }
}