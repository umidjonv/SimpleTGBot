using SimpleBot.Infrastructure.Mediator.MQ;

namespace SimpleBot.Infrastructure.Mediator.Receivers
{
    public class Receiver(IMediator rabbitMediator) : IReceiver
    {


        public void Handle<T>(T @event)
        {
            rabbitMediator.Consume();

            throw new NotImplementedException();
        }
    }
}
