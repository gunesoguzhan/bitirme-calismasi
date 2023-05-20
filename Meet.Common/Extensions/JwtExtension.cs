using System.Text;
using Meet.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Meet.Common.Extensions;

public static class JwtExtension
{
    public static IServiceCollection AddJwtAuthenticationWithSettings(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        ArgumentNullException.ThrowIfNull(configuration);
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        ArgumentNullException.ThrowIfNull(jwtSettings);
        services.AddAuthentication().AddJwtBearer("jsonwebtoken", options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
                ClockSkew = TimeSpan.Zero
            };
        });
        return services;
    }
}