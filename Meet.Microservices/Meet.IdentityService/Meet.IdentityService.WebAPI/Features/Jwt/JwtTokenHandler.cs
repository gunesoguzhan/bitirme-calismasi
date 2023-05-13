using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Meet.IdentityService.WebAPI.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Meet.IdentityService.WebAPI.Features.Jwt;

public class JwtTokenHandler
{
    private readonly IConfiguration _configuration;

    public JwtTokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user, TimeSpan tokenExpiration)
    {
        //Get Jwt settings from config. If not found throw ArgumentNullException.
        var jwtSettings = _configuration.GetSection(nameof(Jwt)).Get<IdentityService.WebAPI.Settings.Jwt>();
        if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecurityKey))
            throw new ArgumentNullException(nameof(Jwt), $"Configuration setting '{nameof(Jwt)}' is missing or has a null value.");

        //Set credentials.
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //Set token claims.
        var claims = new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Username)
        };

        //Generate token.
        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            expires: DateTime.Now.Add(tokenExpiration),
            notBefore: DateTime.Now,
            claims: claims,
            signingCredentials: credentials);

        //Return token string.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}