using SimpleBot.Infrastructure.Mediator.Events;

namespace SimpleBot.Infrastructure.Mediator.MQ
{
    public interface IMediator
    {
        void RegisterHandler<T>(Action<T> handler) where T : BaseEvent;

        void Publish(byte[] message);

        void Consume<T>(Action<T> action) where T : BaseEvent;

    }
}
