namespace MicroSwarm.Templates
{
    public static class RepositoryActorTemplate
    {
        public static string RenderHeader(string solutionName, string serviceName)
        {
            return
$$"""
using Akka.Actor;
using {{serviceName}}.Entities;
using Microsoft.EntityFrameworkCore;
using {{solutionName}}Core.Actors;

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

            Receive<Func<{{serviceName}}Root, bool>>(filter =>
            {
                try
                {
""";
        }

        public static string RenderSetHeader(string serviceName)
        {
            return $"var entities = _context.Set<{serviceName}Root>().AsNoTracking()";
        }

        public static string RenderSetFooter()
        {
            return ".Where(filter);";
        }

        public static string RenderFooter()
        {
            return
"""
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