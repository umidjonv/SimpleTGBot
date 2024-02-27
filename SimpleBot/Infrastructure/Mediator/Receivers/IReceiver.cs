using SimpleBot.Infrastructure.Events;
using SimpleBot.Infrastructure.Mediator.Events;

namespace SimpleBot.Infrastructure.Mediator.Receivers
{
    public interface IReceiver
    {
        void Handle(StartEvent @event);
        void Handle(ProductEvent @event);

    }
}
