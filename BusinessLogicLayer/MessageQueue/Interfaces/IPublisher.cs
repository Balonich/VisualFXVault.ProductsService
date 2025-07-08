namespace BusinessLogicLayer.MessageQueue.Interfaces;

public interface IPublisher
{
    void Publish<T>(T message, string routingKey) where T : class;
}