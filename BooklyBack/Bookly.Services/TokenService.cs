using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookly.Services;

public interface ITokenService
{
    string GerarAccessToken(string userId);
    string GerarRefreshToken(string userId);
    string? ValidarRefreshToken(string token);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GerarAccessToken(string userId)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:AccessSecret"]!)
        );

        var token = new JwtSecurityToken(
            claims: new[] { new Claim("user_id", userId) },
            expires: DateTime.UtcNow.AddHours(
                double.Parse(_config["Jwt:AccessExpirationHours"] ?? "8")
            ),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GerarRefreshToken(string userId)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:RefreshSecret"]!)
        );

        var token = new JwtSecurityToken(
            claims: new[] { new Claim("user_id", userId) },
            expires: DateTime.UtcNow.AddDays(
                double.Parse(_config["Jwt:RefreshExpirationDays"] ?? "7")
            ),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string? ValidarRefreshToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:RefreshSecret"]!);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(x => x.Type == "user_id").Value;
        }
        catch
        {
            return null;
        }
    }
}
