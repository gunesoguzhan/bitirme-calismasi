namespace Meet.IdentityService.WebAPI.Settings;

public class CorsSettings
{
    public string[] AllowedHosts { get; init; } = null!;
    public string[] AllowedHeaders { get; init; } = null!;
    public string[] AllowedMethods { get; init; } = null!;
}