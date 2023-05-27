namespace Meet.Common.Settings;

public class RabbitMQSettings
{
    public string Host { get; init; } = null!;
    public int Port { get; init; }
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public List<string>? Queues { get; set; }
}