namespace MicroSwarm.Templates
{
    public static class ActorServiceTemplate
    {
        public static string Render(string serviceName)
        {
            return
$$"""
using System.Diagnostics;
using Akka.Actor;
using Akka.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace {{serviceName}}Core.Actors
{
    public class ActorService<A>(IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime, IConfiguration configuration)
        : IHostedService, IActorBridge
        where A : ReceiveActor, new()
    {
        private ActorSystem? _actorSystem = null;
        private IActorRef? _actorRef = null;

        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IHostApplicationLifetime _appLifetime = appLifetime;
        private readonly IConfiguration _configuration = configuration;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var bootstrap = BootstrapSetup.Create();

            // enable DI support inside this ActorSystem, if needed
            var diSetup = DependencyResolverSetup.Create(_serviceProvider);

            // merge setups
            var actorSystemSetup = bootstrap.And(diSetup);

            // start the actor system
            _actorSystem = ActorSystem.Create("{{serviceName}}", actorSystemSetup);
            _actorRef = _actorSystem.ActorOf<A>("controller-actor");

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _actorSystem.WhenTerminated.ContinueWith(_ =>
            {
                _appLifetime.StopApplication();
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }

        public void Tell(object message)
        {
            Debug.Assert(_actorRef != null);
            _actorRef.Tell(message);
        }

        public Task<IActorResult> Ask(object message)
        {
            Debug.Assert(_actorRef != null);
            return _actorRef.Ask<IActorResult>(message);
        }
    }
}
""";
        }
    }
}