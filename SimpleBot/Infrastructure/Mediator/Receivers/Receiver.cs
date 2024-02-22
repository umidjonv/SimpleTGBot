using SimpleBot.Infrastructure.Events;
using SimpleBot.Infrastructure.Mediator.Events;
using SimpleBot.Infrastructure.Mediator.MQ;

namespace SimpleBot.Infrastructure.Mediator.Receivers
{
    public class Receiver(IMediator rabbitMediator) : IReceiver
    {


        public void Handle<T>(T @event) where T : BaseEvent
        {
            

        }

        public void Handle(StartEvent @event)
        {
            
        }

        public void Handle(ProductEvent @event)
        {
            
        }
    }
}
