namespace MicroSwarm.TaskHandlers
{
    public interface IResult
    {
        public bool Ok { get; set; }
        public string Value { get; set; }
        public static IResult OkResult(string value)
        {
            return new Result { Ok = true, Value = value };
        }
        public static IResult BadResult(string value)
        {
            return new Result { Ok = false, Value = value };
        }
    }

    public class Result : IResult
    {
        public bool Ok { get; set; } = true;
        public string Value { get; set; } = "";
    }
}