using SimpleBot.Infrastructure.Events;
using SimpleBot.Infrastructure.Mediator;
using SimpleBot.Infrastructure.Mediator.MQ;
using SimpleBot.Infrastructure.Mediator.Receivers;
using SimpleBot.Infrastructure.Services;
using SimpleBot.Models;
using SimpleBot.Services;
using Telegram.Bot;

namespace SimpleBot.Middlewares
{
    public static class InjectionServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) 
        {
            var botConfig = configuration.GetBotConfig();
            var rabbitMqConfig = configuration.GetRabbitMQConfiguration();
            //services.AddHostedService<SearchHostedService>();
            var provider = services.BuildServiceProvider();
            services.AddSingleton(botConfig);
            services.AddSingleton(rabbitMqConfig);

            services.AddHttpClient("telegram_bot_client")
                .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                {

                    TelegramBotClientOptions options = new(botConfig.Token);
                    return new TelegramBotClient(options, httpClient);
                });

            services.AddScoped<UpdateHandler>();
            services.AddScoped<ReceiverService>();
            services.AddHostedService<PollingService>();

            services.AddSingleton<IReceiver, Receiver>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();
            services.AddSingleton<IMediator, RabbitMediator>();
            //services.AddHandlers(provider);
            
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

        private static IServiceCollection AddHandlers(this IServiceCollection services, IServiceProvider provider)
        {
            
            //var receiver = scope.ServiceProvider.GetRequiredService<IReceiver>();
            //var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            //var dispatcher = new EventDispatcher(mediator);
            //dispatcher.RegisterHandler<StartEvent>(receiver.Handle);
            //dispatcher.RegisterHandler<ProductEvent>(receiver.Handle);
            //services.AddSingleton<IEventDispatcher>(_=>dispatcher);
            return services;
        }

    }
}
