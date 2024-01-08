namespace MicroSwarm.Templates
{
    public static class IActorBridgeTemplate
    {
        public static string Render(string serviceName)
        {
            return
$$"""
namespace {{serviceName}}Core.Actors
{
    public interface IActorBridge
    {
        void Tell(object message);
        Task<IActorResult> Ask(object message);
    }
}
""";
        }
    }
}