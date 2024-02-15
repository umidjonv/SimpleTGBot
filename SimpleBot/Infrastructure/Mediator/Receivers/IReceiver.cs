namespace SimpleBot.Infrastructure.Mediator.Receivers
{
    public interface IReceiver
    {
        void Handle<T>(T @event);
    }
}
