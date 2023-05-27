namespace Meet.Common.RabbitMQ;

public interface IConsumer
{
    string QueueName { get; set; }
    void ConsumeMessage(string message);
}