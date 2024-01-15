namespace MicroSwarm.Templates
{
    public static class QueryMapperActorTemplate
    {
        public static string RenderHeader(string solutionName, string serviceName)
        {
            return
$$"""
using Akka.Actor;
using {{serviceName}}.Entities;
using {{solutionName}}Core;
using {{solutionName}}Core.Actors;

namespace {{serviceName}}.Actors
{
    public class QueryMapperActor : ReceiveActor
    {
        private readonly IActorRef _serializeActor;

        public QueryMapperActor()
        {
            _serializeActor = Context.ActorOf(Props.Create<QuerySerializeActor>(), "serialize-actor");

            Receive<IEnumerable<{{serviceName}}Root>>(entities =>
            {
                List<{{serviceName}}Aggregate> aggregates = [];
                foreach (var entity in entities)
                {
                    aggregates.Add(new {{serviceName}}Aggregate
                    {
""";
        }

        public static string RenderFooter()
        {
            return
"""
                    });
                }
                var result = _serializeActor.Ask<IActorResult>(aggregates).Result;
                Sender.Tell(result);
            });
        }
    }
}
""";
        }
    }
}