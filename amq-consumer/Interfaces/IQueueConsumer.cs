namespace Interfaces
{
    public interface IQueueConsumer
    {
        Task StartConsumeAsync(CancellationToken cancellationToken);
    }
}
