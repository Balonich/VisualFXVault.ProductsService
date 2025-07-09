namespace BusinessLogicLayer.MessageQueue.Interfaces;

public interface IPublisher
{
    Task PublishAsync<T>(T message, string routingKey) where T : class;
}