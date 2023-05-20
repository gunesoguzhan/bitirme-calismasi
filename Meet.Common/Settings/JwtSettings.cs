namespace Meet.Common.Settings;

public class JwtSettings
{
    public string Audience { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string SecurityKey { get; init; } = null!;
}