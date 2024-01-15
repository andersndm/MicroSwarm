namespace MicroSwarm.Templates
{
    public static class QueryMapperActorTemplate
    {
        public static string Render(string solutionName, string serviceName, string rootName)
        {
            return
$$"""
using Akka.Actor;
using {{solutionName}}Core.Aggregates;
using {{solutionName}}Core.Actors;

namespace {{serviceName}}.Actors
{
    public class QueryMapperActor : ReceiveActor
    {
        private readonly IActorRef _serializeActor;

        public QueryMapperActor()
        {
            _serializeActor = Context.ActorOf(Props.Create<QuerySerializeActor>(), "serialize-actor");

            Receive<IEnumerable<{{rootName}}>>(entities =>
            {
                var result = _serializeActor.Ask<IActorResult>(entities).Result;
                Sender.Tell(result);
            });
        }
    }
}
""";
        }
    }
}