namespace Meet.ChatService.WebAPI.Settings;

public class Jwt
{
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SecurityKey { get; init; }
}