using SimpleBot.Infrastructure.Events;
using SimpleBot.Infrastructure.Mediator.Events;

namespace SimpleBot.Infrastructure.Mediator.Receivers
{
    public interface IReceiver
    {
        Task Handle(StartEvent @event);
        Task Handle(ProductEvent @event);

    }
}
