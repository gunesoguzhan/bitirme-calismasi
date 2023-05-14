namespace Meet.IdentityService.WebAPI.Settings;

public class RabbitMQSettings
{
    public string Host { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; set; } = null!;
}