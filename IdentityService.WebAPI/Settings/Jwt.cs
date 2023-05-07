namespace IdentityService.WebAPI.Settings;

class Jwt
{
    public string? SecurityKey { get; init; }
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
}