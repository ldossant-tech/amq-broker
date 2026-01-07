using Microsoft.AspNetCore.Mvc;
using Interfaces;

namespace Controllers
{
    [Route("api/amq")]
    public class AmqConsumerController : BaseController
    {
        private readonly IQueueConsumer _consumer;

        public AmqConsumerController(IQueueConsumer consumer)
        {
            _consumer = consumer;
        }

        [HttpPost("consume")]
        public async Task<IActionResult> Consume(CancellationToken cancellationToken)
        {
            await _consumer.StartConsumeAsync(cancellationToken);
            return Success("Consumo iniciado com sucesso");
        }
    }
}
