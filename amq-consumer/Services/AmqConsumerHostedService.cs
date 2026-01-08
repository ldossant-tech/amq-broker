using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class AmqConsumerHostedService : BackgroundService
    {
        private readonly IQueueConsumer _consumer;
        private readonly ILogger<AmqConsumerHostedService> _logger;

        public AmqConsumerHostedService(
            IQueueConsumer consumer,
            ILogger<AmqConsumerHostedService> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando consumer AMQ junto com a aplicação");
            await _consumer.StartConsumeAsync(stoppingToken);
        }
    }
}
