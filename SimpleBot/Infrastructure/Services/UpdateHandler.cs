using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using System.Threading;
using SimpleBot.Infrastructure.Mediator.MQ;
using SimpleBot.Infrastructure.Events;
namespace SimpleBot.Infrastructure.Services
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly IMediator mediator;

        public UpdateHandler(IMediator mediator) 
        {
            this.mediator = mediator;
        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
                { EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
                //{ CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
                //{ InlineQuery: { } inlineQuery } => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
                //{ ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            return Task.CompletedTask;
        }

        private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Unknown data receiver: {update.Type}");
            return Task.CompletedTask;
        }

        private async Task BotOnMessageReceived(Message? message, CancellationToken cancellationToken)
        {
            if (message.Text is not { } messageText)
                return;

            var @event = new StartEvent 
            {
                ChatIdentifier = message.Chat.Id,
                Id = Guid.NewGuid(),
                Name = message.Text,
            };

            mediator.Publish(@event);

            //var action = messageText.Split(' ')[0] switch
            //{
            //    "/products" => "Please enter search text:",

            //    _ => Usage(_botClient, message, cancellationToken)
            //};

        }
        

        private async Task Usage(ITelegramBotClient client, Message? message, CancellationToken cancellationToken)
        {
            
        }
    }
}
