namespace SimpleBot.Infrastructure.Mediator.Handlers
{
    public interface IPublisher
    {
        void Publish<T>(T message);
    }
}
