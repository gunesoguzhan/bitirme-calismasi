namespace Meet.ApiGateway.WebAPI.Settings;

public class CorsSettings
{
    public string[] AllowedOrigins { get; init; } = null!;
    public string[] AllowedHeaders { get; init; } = null!;
    public string[] AllowedMethods { get; init; } = null!;
}