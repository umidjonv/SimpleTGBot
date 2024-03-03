using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleBot.Infrastructure.Events;
using SimpleBot.Infrastructure.Mediator.Events;
using SimpleBot.Infrastructure.Mediator.Receivers;
using SimpleBot.Models;
using System.Text;
using System.Text.Json;
using System.Reflection.Metadata;
using System;
namespace SimpleBot.Infrastructure.Mediator.MQ
{
    public class RabbitMediator : IMediator
    {
        private readonly IModel _channel;
        private readonly IReceiver _receiver;
        private readonly IEventDispatcher _dispatcher;
        private const string EventsExchange = "events";
        private const string EventsRoute = "events_route";
        private const string EventsQueue = "events_queue";
        private readonly IConnection _connection;

        
        public RabbitMediator(RabbitMQConfiguration rabbitMQConfiguration, IReceiver receiver, IEventDispatcher dispatcher)
        {
            var factory = new ConnectionFactory
            {
                Password = rabbitMQConfiguration.RabbitMQPassword,
                UserName = rabbitMQConfiguration.RabbitMQUser,
                Port = rabbitMQConfiguration.RabbitMQPort,
                VirtualHost = rabbitMQConfiguration.RabbitMQVirtualHost,
                HostName = rabbitMQConfiguration.RabbitMQConnection,
                DispatchConsumersAsync = true
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _receiver = receiver;
            _dispatcher = dispatcher;
            RegisteringHandlers();
        }

        public void RegisteringHandlers()
        {
            RegisterHandler<StartEvent>(_receiver.Handle);
            RegisterHandler<ProductEvent>(_receiver.Handle);
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

        public void RegisterHandler<T>(Func<T, Task> handler) where T: BaseEvent
        {
            
            this.Consume(handler);
        }

        public void Publish<T>(T @event) where T : BaseEvent
        {
            var queueName = $"{EventExchange}_{typeof(T).Name}";
            ExchangeDeclare();

            var messageJson = JsonSerializer.Serialize<T>(@event);

            var message = Encoding.UTF8.GetBytes(messageJson);

            _channel.BasicPublish(EventsExchange, EventsRoute, basicProperties: null, body: message);
        }

        public void Consume<T>(Func<T, Task> action) where T : BaseEvent
        {
            _channel.ExchangeDeclare(EventsExchange, ExchangeType.Direct);

            var queueName = $"{EventsQueue}_{typeof(T).Name}";

            _channel.QueueDeclare(queueName, true, false, true);

            _channel.QueueBind(queueName, EventsExchange, EventsRoute);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (sender, data) =>
            {
                var eventMessage = Encoding.UTF8.GetString(data.Body.ToArray());

                try
                {
                    var @event = JsonSerializer.Deserialize<T>(eventMessage, new JsonSerializerOptions { MaxDepth = 3 });
                    if (@event is null)
                        throw new ArgumentNullException(nameof(@event), "Could not find event from consumer!");

                    await Task.Run(() => { action.Invoke(@event); });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                

                
                //var handlerMethod = _receiver.GetType().GetMethod("Handle", new Type[] { @event.GetType() });
                //if (handlerMethod is null)
                //    throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");

                //await (handlerMethod.Invoke(_receiver, new object[] { @event }) as Task);


            };

            _channel.BasicConsume(queueName, false, consumer);
        }

    }
}
