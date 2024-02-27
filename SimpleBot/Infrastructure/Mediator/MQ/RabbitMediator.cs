using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleBot.Infrastructure.Mediator.Events;
using SimpleBot.Infrastructure.Mediator.Receivers;
using SimpleBot.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace SimpleBot.Infrastructure.Mediator.MQ
{
    public class RabbitMediator : IMediator
    {
        private readonly IModel _channel;
        private readonly IReceiver _receiver;
        private const string EventsExchange = "events";
        private const string EventsRoute = "events_route";
        private const string EventsQueue = "events_queue";
        

        
        public RabbitMediator(RabbitMQConfiguration rabbitMQConfiguration, IReceiver receiver)
        {
            var factory = new ConnectionFactory
            {
                Password = rabbitMQConfiguration.RabbitMQPassword,
                UserName = rabbitMQConfiguration.RabbitMQUser,
                Port = rabbitMQConfiguration.RabbitMQPort,

                HostName = rabbitMQConfiguration.RabbitMQConnection
            };
            using var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _receiver = receiver;
        }

        public void Dispose()
        {
            _channel.Dispose();
        }

        private void ExchangeDeclare()
        {
            _channel.ExchangeDeclare(exchange: EventsExchange, type: ExchangeType.Direct);

        }

        public string QueueDeclare()
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: EventsExchange,
                              routingKey: string.Empty);

            return queueName;
        }

        public void RegisterHandler<T>(Action<T> handler) where T: BaseEvent
        {
            this.Consume(handler);
        }

        public void Publish(byte[] message)
        {
            ExchangeDeclare();

            _channel.BasicPublish(EventsExchange, EventsRoute, basicProperties: null, body: message);
        }

        public void Consume<T>(Action<T> action) where T : BaseEvent
        {
            _channel.ExchangeDeclare(EventsExchange, ExchangeType.Direct);

            _channel.QueueDeclare(EventsQueue, true, false, true);

            _channel.QueueBind(EventsQueue, EventsExchange, EventsRoute);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (sender, data) =>
            {
                var @event = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data.Body.ToArray()));
                if (@event is null)
                    throw new ArgumentNullException(nameof(@event), "Could not find event from consumer!");

                await Task.Run(() => { action.Invoke(@event); });

                
                //var handlerMethod = _receiver.GetType().GetMethod("Handle", new Type[] { @event.GetType() });
                //if (handlerMethod is null)
                //    throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");

                //await (handlerMethod.Invoke(_receiver, new object[] { @event }) as Task);


            };

            _channel.BasicConsume(EventsQueue, false, consumer);
        }

    }
}
