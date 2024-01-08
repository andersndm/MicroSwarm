namespace MicroSwarm.Templates
{
    public static class CmdActorTemplate
    {
        public static string Render(string solutionName, string serviceName)
        {
            return
$$"""
using System.Text.Json;
using Akka.Actor;
using {{solutionName}}Core.Actors;

namespace {{serviceName}}.Actors
{
    public class CmdActor : ReceiveActor
    {
        public CmdActor()
        {
            Receive<JsonDocument>(cmd =>
            {
                Sender.Tell(IActorResult.BadResult("{{serviceName}} can't handle this cmd: \"" + cmd.RootElement.ToString() + "\""), Self);
            });
        }
    }
}
""";
        }
    }
}