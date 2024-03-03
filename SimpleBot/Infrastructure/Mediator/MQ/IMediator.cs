using SimpleBot.Infrastructure.Mediator.Events;

namespace SimpleBot.Infrastructure.Mediator.MQ
{
    public interface IMediator
    {
        void RegisterHandler<T>(Func<T, Task> handler) where T : BaseEvent;

        void Publish<T>(T @event) where T : BaseEvent;

        void Consume<T>(Func<T, Task> action) where T : BaseEvent;

    }
}
