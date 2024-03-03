
using SimpleBot.Infrastructure.Services;

namespace SimpleBot.Services
{
    public class PollingService: PollingServiceBase<ReceiverService>
    {
        public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger)
        {
        }
    }
}
