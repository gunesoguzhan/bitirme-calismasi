using System.Text;
using System.Text.Json;
using Meet.Common.Settings;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Meet.Common.RabbitMQ;

public class RabbitMQClient
{
    private IConnection _connection;
    private IModel _channel;
    private readonly ILogger<RabbitMQClient> _logger;

    public RabbitMQClient(RabbitMQSettings rabbitMQSettings, ILogger<RabbitMQClient> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory()
        {
            HostName = rabbitMQSettings.Host,
            Port = rabbitMQSettings.Port,
            UserName = rabbitMQSettings.Username,
            Password = rabbitMQSettings.Password
        };
        _connection = factory.CreateConnection();
        var url = $"amqp://{rabbitMQSettings.Username}:{rabbitMQSettings.Password}@{rabbitMQSettings.Host}:{rabbitMQSettings.Port}";
        _logger.LogDebug("AMQP: connected to {url}", url);
        _channel = _connection.CreateModel();
        _logger.LogDebug("AMQP: channel created.");
        if (rabbitMQSettings.Queues == null)
            return;
        foreach (var queue in rabbitMQSettings.Queues)
        {
            _channel.QueueDeclare(queue, true, false, false);
            _logger.LogDebug("AMQP: queue declared. QueueName {queue}", queue);
        }
    }

    public void PublishMessage<T>(string queue, T @object)
    {
        var body = JsonSerializer.Serialize(@object);
        var message = Encoding.UTF8.GetBytes(body);
        _channel.BasicPublish(String.Empty, queue, null, message);
        _logger.LogDebug("AMQP: message sended to queue. QueueName {queue}", queue);
    }

    public void Consume(IConsumer cons)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (sender, args) =>
        {
            _logger.LogDebug("AMQP: message received. QueueName: {queueName}", cons.QueueName);
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            cons.ConsumeMessage(message);
            _channel.BasicAck(args.DeliveryTag, false);
            _logger.LogDebug("AMQP: acked. QueueName: {queueName}", cons.QueueName);
        };
        string consumerTag = _channel.BasicConsume(cons.QueueName, false, consumer);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
        _logger.LogDebug("AMQP: channel and connection disposed.");
    }
}