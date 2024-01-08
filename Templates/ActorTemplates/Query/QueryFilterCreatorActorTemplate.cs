namespace MicroSwarm.Templates
{
    public static class QueryFilterCreatorActorTemplate
    {
        public static string Render(string solutionName, string serviceName)
        {
            return
$$"""
using Akka.Actor;
using Akka.DependencyInjection;
using {{solutionName}}Core.Actors;

namespace {{serviceName}}.Actors
{
    public class FilterCreatorActor : ReceiveActor
    {
        private readonly IActorRef _repositoryActor;
        public FilterCreatorActor()
        {
            var repoProps = DependencyResolver.For(Context.System).Props<RepositoryActor>();
            _repositoryActor = Context.ActorOf(repoProps, "repository-actor");

            Receive<string>(filterStr =>
            {
                try
                {
                    var filter = new {{serviceName}}Filter().FromJson(filterStr).CreateFilter();
                    var result = _repositoryActor.Ask<IActorResult>(filter).Result;
                    Sender.Tell(result);
                }
                catch (Exception e)
                {
                    // get the innermost exception
                    while (e.InnerException != null)
                    {
                        e = e.InnerException;
                    }
                    Sender.Tell(IActorResult.BadResult(e.Message));
                }
            });
        }
    }
}
""";
        }
    }
}