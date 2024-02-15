namespace SimpleBot.Infrastructure.Mediator.MQ
{
    public interface IMediator
    {
        void RegisterHandler<T>(Action<T> handler);
        void Publish(byte[] message);

        void Consume();

    }
}
