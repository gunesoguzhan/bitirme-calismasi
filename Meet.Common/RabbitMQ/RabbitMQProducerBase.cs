using Meet.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

public abstract class RabbitMQProducerBase
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly ILogger<RabbitMQProducerBase> _logger;

    protected RabbitMQProducerBase(IConfiguration configuration, ILogger<RabbitMQProducerBase> logger)
    {
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
    }

    protected void PublishMessage(string queueName, string message)
    {
        channel.QueueDeclare(queueName, true, false, false, null);
        _logger.LogDebug("AMQP: Queue declared: QueueName: {queueName}", queueName);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish("", queueName, null, body);
        _logger.LogDebug("AMQP: Message published to the queue. QueueName: {queueName}", queueName);
    }

    public void Dispose()
    {
        channel?.Dispose();
        _logger.LogDebug("AMQP: Channel disposed.");
        connection?.Dispose();
        _logger.LogDebug("AMQP: Connection disposed.");
    }
}
