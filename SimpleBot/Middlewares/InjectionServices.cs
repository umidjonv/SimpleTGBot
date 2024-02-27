using SimpleBot.Infrastructure.Mediator;
using SimpleBot.Infrastructure.Mediator.MQ;
using SimpleBot.Infrastructure.Mediator.Receivers;
using SimpleBot.Models;
using SimpleBot.Services;

namespace SimpleBot.Middlewares
{
    public static class InjectionServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) 
        {
            var botConfig = configuration.GetBotConfig();
            var rabbitMqConfig = configuration.GetRabbitMQConfiguration();
            //services.AddHostedService<SearchHostedService>();

            services.AddSingleton(botConfig);
            services.AddSingleton(rabbitMqConfig);
            
            services.AddSingleton<IReceiver, Receiver>();
            services.AddSingleton<IMediator, RabbitMediator>();
            services.AddSingleton<RegisterHandler>();
            return services;
        }

        public static BotConfig GetBotConfig(this IConfiguration configuration) 
        {
            var botConfig = new BotConfig();
            configuration.Bind("BotConfig", botConfig);
            return botConfig;
        }
        
        public static RabbitMQConfiguration GetRabbitMQConfiguration(this IConfiguration configuration) 
        {
            var config = new RabbitMQConfiguration();
            configuration.Bind("RabbitMQConfiguration", config);
            return config;
        }

    }
}
