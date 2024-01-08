namespace MicroSwarm.Templates
{
    public static class QueryActorTemplate
    {
        public static string Render(string solutionName, string serviceName)
        {
            return
$$"""
using Akka.Actor;
using {{solutionName}}Core.Actors;

namespace {{serviceName}}.Actors
{
    public class QueryActor : ReceiveActor
    {
        private readonly IActorRef _filterCreator;

        public QueryActor()
        {
            _filterCreator = Context.ActorOf(Props.Create<FilterCreatorActor>(), "filter-creator");

            Receive<string>(filterStr =>
            {
                var result = _filterCreator.Ask<IActorResult>(filterStr).Result;
                Sender.Tell(result);
            });
        }
    }
}
""";
        }
    }
}