using Telegram.Bot.Types;

namespace SimpleBot.Infrastructure.Mediator.Events
{
    public class BaseEvent:IBotEvent
    {
        public Guid Id { get; set; }

        public long? ChatIdentifier { get; set; }

        public string Name { get; set; }
        public Type EventType { get; set; }

    }
}
