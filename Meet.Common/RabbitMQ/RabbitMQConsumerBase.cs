using Meet.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Meet.Common.RabbitMQ;

public abstract class RabbitMQConsumerBase : IHostedService
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly ILogger<RabbitMQConsumerBase> _logger;
    private readonly string _queueName;

    protected RabbitMQConsumerBase(string queueName, IConfiguration configuration, ILogger<RabbitMQConsumerBase> logger)
    {
        _queueName = queueName;
        _logger = logger;
        var rabbitMQSettings = configuration
            .GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
        ArgumentNullException.ThrowIfNull(rabbitMQSettings);
        var factory = new ConnectionFactory()
        {
            HostName = rabbitMQSettings.Host,
            Port = rabbitMQSettings.Port,
            UserName = rabbitMQSettings.Username,
            Password = rabbitMQSettings.Password
        };

        connection = factory.CreateConnection();
        _logger.LogDebug("AMQP: Connected to: {connection}", $"amqp://{rabbitMQSettings.Username}:{rabbitMQSettings.Password}@{rabbitMQSettings.Host}:{rabbitMQSettings.Port}");
        channel = connection.CreateModel();
        _logger.LogDebug("AMQP: Channel created.");

        channel.QueueDeclare(_queueName, true, false, false, null);
        _logger.LogDebug("AMQP: Queue declared: QueueName: {queueName}", _queueName);
    }

    public abstract void HandleMessage(string message);

    private void Dispose()
    {
        channel?.Dispose();
        _logger.LogDebug("AMQP: Channel disposed.");
        connection?.Dispose();
        _logger.LogDebug("AMQP: Connection disposed.");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            _logger.LogDebug("AMQP: Message received from queue. QueueName: {queueName}", _queueName);
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            HandleMessage(message);
            channel.BasicAck(ea.DeliveryTag, false);
            _logger.LogDebug("AMQP: Acked. QueueName: {queueName}", _queueName);
        };
        channel.BasicConsume(_queueName, false, consumer);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Dispose();
        return Task.CompletedTask;
    }
}
