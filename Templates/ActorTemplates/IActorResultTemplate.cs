namespace MicroSwarm.Templates
{
    public static class IActorResultTemplate
    {
        public static string Render(string serviceName)
        {
            return
$$"""
namespace {{serviceName}}Core.Actors
{
    public interface IActorResult
    {
        public bool Ok { get; set; }
        public string Value { get; set; }
        public static IActorResult OkResult(string value)
        {
            return new ActorResult { Ok = true, Value = value };
        }

        public static IActorResult BadResult(string value)
        {
            return new ActorResult { Ok = false, Value = value };
        }
    }

    public class ActorResult : IActorResult
    {
        public bool Ok { get; set; } = true;
        public string Value { get; set; } = "";
    }
}
""";
        }
    }
}