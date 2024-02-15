namespace SimpleBot.Infrastructure.Mediator.Handlers
{
    public interface IMediator
    {
        void RegisterHandler<T>(T handler);


    }
}
