namespace MicroSwarm.Templates
{
    public static class RepositoryActorTemplate
    {
        public static string Render(string solutionName, string serviceName, string rootName)
        {
            return
$$"""
using Akka.Actor;
using Microsoft.EntityFrameworkCore;
using {{solutionName}}Core.Actors;
using {{solutionName}}Core.Aggregates;

namespace {{serviceName}}.Actors
{
    public class RepositoryActor : ReceiveActor
    {
        private readonly {{serviceName}}DbContext _context;

        private readonly IActorRef _queryMapper;

        public RepositoryActor({{serviceName}}DbContext context)
        {
            _context = context;

            _queryMapper = Context.ActorOf(Props.Create<QueryMapperActor>(), "query-mapper");

            Receive<Func<{{rootName}}, bool>>(filter =>
            {
                try
                {
                    var entities = _context.Set<{{rootName}}>().AsNoTracking().Where(filter);
                    var result = _queryMapper.Ask<IActorResult>(entities).Result;
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