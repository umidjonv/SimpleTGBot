namespace SimpleBot.Infrastructure.Mediator.Handlers
{
    public interface IConsumer
    {
        void Handle(IConsumer consumer);
    }
}
