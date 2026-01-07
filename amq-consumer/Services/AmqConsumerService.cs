using Interfaces;
using Microsoft.Extensions.Logging;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace Services
{
    public class AmqConsumerService : IQueueConsumer
    {
        private readonly ILogger<AmqConsumerService> _logger;

        private const string BrokerUri = "tcp://localhost:61616";
        private const string QueueName = "minha.fila";

        public AmqConsumerService(ILogger<AmqConsumerService> logger)
        {
            _logger = logger;
        }

        public async Task StartConsumeAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Conectando ao AMQ...");

            var factory = new ConnectionFactory(BrokerUri);

            using var connection = factory.CreateConnection();
            connection.Start();

            using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            using var destination = session.GetQueue(QueueName);
            using var consumer = session.CreateConsumer(destination);

            _logger.LogInformation("Consumindo mensagens da fila {Queue}", QueueName);

            while (!cancellationToken.IsCancellationRequested)
            {
                var message = consumer.Receive(TimeSpan.FromSeconds(1));

                if (message is ITextMessage textMessage)
                {
                    _logger.LogInformation("Mensagem recebida: {Message}", textMessage.Text);
                }

                await Task.Delay(100, cancellationToken);
            }

            _logger.LogInformation("Consumer AMQ finalizado");
        }
    }
}
