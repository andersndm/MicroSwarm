namespace MicroSwarm.Templates
{
    public static class QuerySerializeActorTemplate
    {
        public static string Render(string solutionName, string serviceName)
        {
            return
$$"""
using System.Text.Json;
using Akka.Actor;
using {{solutionName}}Core;
using {{solutionName}}Core.Actors;

namespace {{serviceName}}.Actors
{
    public class QuerySerializeActor : ReceiveActor
    {
        public QuerySerializeActor()
        {
            Receive<IEnumerable<{{serviceName}}Aggregate>>(aggregates =>
            {
                try
                {
                    var result = JsonSerializer.Serialize(aggregates);
                    Sender.Tell(IActorResult.OkResult(result));
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