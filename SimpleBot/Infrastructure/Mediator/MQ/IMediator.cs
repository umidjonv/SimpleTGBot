namespace SimpleBot.Infrastructure.Mediator.MQ
{
    public interface IMediator
    {
        
        void Publish(byte[] message);

        void Consume(Action<byte[]?> action);

    }
}
