using SimpleBot.Infrastructure.Mediator.Events;
using SimpleBot.Infrastructure.Mediator.MQ;
using SimpleBot.Models;
using System.Text;

namespace SimpleBot.Infrastructure.Mediator.Handlers
{
    public class Publisher(RabbitMediator rabbitMediator) : IPublisher
    {
        public void Publish<T>(T message) where T : BaseEvent
        {
            rabbitMediator.ExchangeDeclare();

            var eventType = typeof(T);
            var serializedJson = System.Text.Json.JsonSerializer.Serialize<T>(message);
            
            var payload = Encoding.UTF8.GetBytes(serializedJson);
            rabbitMediator.PublishEvent(payload);
            

        }
    }
}
