using Intaker.Assignment.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Intaker.Assignment.Application.Integration;

public class ServiceBusHandler : IServiceBusHandler, IDisposable
{
    public const string QueueName = "Tasks";

    private readonly string _host;
    private readonly string _port;

    private readonly IEventProcessor _eventProcessor;


    private IConnection _connection;
    private IChannel _channel;

    public ServiceBusHandler(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _host = configuration["RabbitMQ:Host"]!;
        _port = configuration["RabbitMQ:Port"]!;

        _eventProcessor = eventProcessor;
    }

    public async Task Initialize(CancellationToken token = default)
    {
        var connectionFactory = new ConnectionFactory()
        {
            HostName = _host,
            Port = int.Parse(_port)
        };

        _connection = await connectionFactory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(queue: QueueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
    }

    public async Task SendMessage(Domain.Task task, CancellationToken token = default)
    {
        string message = JsonSerializer.Serialize(task);
        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties();
        props.ContentType = "application/json";

        await _channel.BasicPublishAsync(exchange: string.Empty,
                             routingKey: QueueName,
                             mandatory: true,
                             basicProperties: props,
                             body: body,
                             cancellationToken: token);
    }

    public async Task ReceiveMessage(CancellationToken token = default)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await _eventProcessor.ProcessEvent(message);
        };

        await _channel.BasicConsumeAsync(queue: QueueName,
                              autoAck: true,
                              consumer: consumer,
                              cancellationToken: token);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
