﻿using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace SimpleBot.Infrastructure
{
    public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService
        where TUpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly TUpdateHandler _updateHandler;
        private readonly ILogger<ReceiverServiceBase<TUpdateHandler>> _logger;

        public ReceiverServiceBase(ITelegramBotClient botClient,
        TUpdateHandler updateHandler, ILogger<ReceiverServiceBase<TUpdateHandler>> logger)
        {
            _botClient = botClient;
            _updateHandler = updateHandler;
            _logger = logger;
        }

        public async Task ReceiveAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true,
            };

            var me = await _botClient.GetMeAsync(stoppingToken);
            _logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "My Awesome Bot");

            // Start receiving updates
            await _botClient.ReceiveAsync(
                updateHandler: _updateHandler,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken);
        }
    }
}
