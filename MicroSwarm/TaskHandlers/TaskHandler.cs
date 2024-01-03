namespace MicroSwarm.TaskHandlers
{
    public interface ITaskHandler<in TIn>
    {
        IResult Handle(TIn input);
    }

    public abstract class TaskHandlerTail<TIn> : ITaskHandler<TIn>
    {
        public abstract IResult Handle(TIn input);
    }

    public abstract class TaskHandler<TIn, TOut> : ITaskHandler<TIn>
    {
        protected ITaskHandler<TOut> _next;

        public void SetNext(ITaskHandler<TOut> next) => _next = next;

        public TaskHandler(ITaskHandler<TOut> next) => SetNext(next);

        public abstract IResult Handle(TIn input);
    }
}