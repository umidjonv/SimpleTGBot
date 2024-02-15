
using SimpleBot.Models;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SimpleBot.Services
{
    public class SearchHostedService(BotConfig botConfig) : BackgroundService
    {
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var botClient = new TelegramBotClient($"{botConfig.Token}");

            

            var me = await botClient.GetMeAsync();
            //botClient.ReceiveAsync()
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
        }
    }
}
