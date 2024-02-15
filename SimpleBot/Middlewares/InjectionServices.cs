using SimpleBot.Models;
using SimpleBot.Services;

namespace SimpleBot.Middlewares
{
    public static class InjectionServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) 
        {
            var botConfig = configuration.GetBotConfig();
            services.AddHostedService<SearchHostedService>();
            services.AddSingleton(botConfig);

            return services;
        }

        public static BotConfig GetBotConfig(this IConfiguration configuration) 
        {
            var botConfig = new BotConfig();
            configuration.Bind("BotConfig", botConfig);
            return botConfig;
        }

    }
}
