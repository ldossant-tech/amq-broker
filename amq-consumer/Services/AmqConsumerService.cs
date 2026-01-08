using Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Services.Configuration;

namespace Services
{
    public class AmqConsumerService : IQueueConsumer
    {
        private readonly ILogger<AmqConsumerService> _logger;
        private readonly AmqBrokerOptions _options;

        public AmqConsumerService(
            ILogger<AmqConsumerService> logger,
            IOptions<AmqBrokerOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task StartConsumeAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // URI FINAL (Host + Port + ExtraParams)
                    var brokerUri = _options.BrokerUri;

                    _logger.LogInformation(
                        "Conectando ao AMQ Broker em {Broker}",
                        brokerUri
                    );

                    var factory = new ConnectionFactory(brokerUri)
                    {
                        UserName = _options.Username,
                        Password = _options.Password
                    };

                    using var connection = factory.CreateConnection();
                    connection.Start();

                    using var session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                    using var destination = session.GetQueue(_options.Queue);
                    using var consumer = session.CreateConsumer(destination);

                    _logger.LogInformation(
                        "Conectado com sucesso. Consumindo fila {Queue}",
                        _options.Queue
                    );

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var message = consumer.Receive(TimeSpan.FromSeconds(1));

                        if (message is ITextMessage textMessage)
                        {
                            _logger.LogInformation(
                                "Mensagem recebida: {Message}",
                                textMessage.Text
                            );
                        }

                        await Task.Delay(100, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Erro ao conectar/consumir AMQ. Tentando novamente em 10 segundos..."
                    );

                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                }
            }

            _logger.LogInformation("Consumer AMQ finalizado");
        }
    }
}
