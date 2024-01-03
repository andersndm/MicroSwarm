using MicroSwarm.TaskHandlers;

namespace MicroSwarm.Pipeline
{
    public interface IPipeline<in TIn>
    {
        IResult Execute(TIn input);
    }

    public class Pipeline<TIn>(ITaskHandler<TIn> handler) : IPipeline<TIn>
    {
        private ITaskHandler<TIn> _handler = handler;

        public IResult Execute(TIn input)
        {
            return _handler.Handle(input);
        }
    }
}