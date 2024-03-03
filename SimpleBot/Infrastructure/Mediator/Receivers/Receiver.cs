using SimpleBot.Infrastructure.Events;
using SimpleBot.Infrastructure.Mediator.Events;
using SimpleBot.Infrastructure.Mediator.MQ;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SimpleBot.Infrastructure.Mediator.Receivers
{
    public class Receiver : IReceiver
    {
        private readonly ITelegramBotClient _client;

        public Receiver(ITelegramBotClient client)
        {
            _client = client;
        }


        public async Task Handle(StartEvent @event)
        {
            await _client.SendTextMessageAsync(
                chatId: @event.ChatIdentifier,
                text: "Start Event received");
            Console.WriteLine("Start Event received");
        }

        public async Task Handle(ProductEvent @event)
        {
            await _client.SendTextMessageAsync(
                chatId: @event.ChatIdentifier,
                text: "Product Event received");
            Console.WriteLine("Product Event received");
        }
    }
}
