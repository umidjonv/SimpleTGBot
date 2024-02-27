using SimpleBot.Infrastructure.Events;
using SimpleBot.Infrastructure.Mediator.MQ;
using SimpleBot.Infrastructure.Mediator.Receivers;

namespace SimpleBot.Infrastructure.Mediator
{
    public class RegisterHandler
    {
        public RegisterHandler(IMediator rabbitMediator, IReceiver receiver)
        {
            rabbitMediator.RegisterHandler<StartEvent>(receiver.Handle);
            rabbitMediator.RegisterHandler<ProductEvent>(receiver.Handle);
        }

    }
}
