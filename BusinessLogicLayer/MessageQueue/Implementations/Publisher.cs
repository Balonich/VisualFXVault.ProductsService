using BusinessLogicLayer.MessageQueue.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace BusinessLogicLayer.MessageQueue.Implementations;

public class Publisher : IPublisher, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public Publisher(IConfiguration configuration)
    {
        _configuration = configuration;

        var hostName = _configuration["RABBITMQ_HOST"]!;
        var port = _configuration["RABBITMQ_PORT"]!;
        var userName = _configuration["RABBITMQ_USER"]!;
        var password = _configuration["RABBITMQ_PASSWORD"]!;

        var connectionFactory = new ConnectionFactory
        {
            HostName = hostName,
            Port = int.Parse(port),
            UserName = userName,
            Password = password
        };

        _connection = connectionFactory.CreateConnectionAsync().Result;

        _channel = _connection.CreateChannelAsync().Result;
    }

    public void Publish<T>(T message, string routingKey) where T : class
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}