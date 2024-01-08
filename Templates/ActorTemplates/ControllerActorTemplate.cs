namespace MicroSwarm.Templates
{
    public static class ControllerActorTemplate
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
    public class {{serviceName}}ControllerActor : ReceiveActor
    {
        private readonly IActorRef _queryActor;
        private readonly IActorRef _cmdActor;

        public {{serviceName}}ControllerActor()
        {
            _queryActor = Context.ActorOf(Props.Create<QueryActor>(), "query-actor");
            _cmdActor = Context.ActorOf(Props.Create<CmdActor>(), "cmd-actor");

            Receive<string>(filterStr =>
            {
                Sender.Tell(_queryActor.Ask<IActorResult>(filterStr).Result);
            });

            Receive<JsonDocument>(cmd =>
            {
                Sender.Tell(_cmdActor.Ask<IActorResult>(cmd).Result);
            });
        }
    }
}
""";
        }
    }
}