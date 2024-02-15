using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleBot.Infrastructure.Mediator.Events;
using SimpleBot.Models;
using System.Threading.Channels;

namespace SimpleBot.Infrastructure.Mediator.MQ
{
    public abstract class RabbitMediator : IDisposable, IMediator
    {
        private readonly IModel _channel;
        private const string EventsExchange = "events";
        private const string EventsRoute = "events_route";
        private const string EventsQueue = "events_queue";
        
        public RabbitMediator(RabbitMQConfiguration rabbitMQConfiguration)
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

        public void RegisterHandler<T>(Action<T> handler)
        {
            
        }

        public void Publish(byte[] message)
        {
            ExchangeDeclare();

            _channel.BasicPublish(EventsExchange, EventsRoute, basicProperties: null, body: message);
        }

        public void Consume(Action<object?> action)
        {
            _channel.ExchangeDeclare(EventsExchange, ExchangeType.Direct);

            _channel.QueueDeclare(EventsQueue, true, false, true);

            _channel.QueueBind(EventsQueue, EventsExchange, EventsRoute);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (sender, @event) =>
            {
                //action.Invoke();                
            };

            _channel.BasicConsume(EventsQueue, false, consumer);
        }

    }
}
