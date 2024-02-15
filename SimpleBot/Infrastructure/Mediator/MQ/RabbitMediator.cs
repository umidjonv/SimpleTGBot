using Newtonsoft.Json.Bson;
using RabbitMQ.Client;
using SimpleBot.Models;
using System.Threading.Channels;

namespace SimpleBot.Infrastructure.Mediator.MQ
{
    public abstract class RabbitMediator : IDisposable
    {
        private readonly IModel _channel;
        private const string CommandExchange = "commands";
        private const string CommandRoute = "commands_route";
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
            _channel.ExchangeDeclare(exchange: CommandExchange, type: ExchangeType.Direct);

        }

        public void PublishEvent(byte[] message)
        {
            ExchangeDeclare();
            _channel.BasicPublish(CommandExchange, CommandRoute, basicProperties: null,body: message);
        }

        public string QueueDeclare()
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: CommandExchange,
                              routingKey: string.Empty);

            return queueName;
        }
    }
}
