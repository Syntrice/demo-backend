using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DemoBackend.Models.JWTs.Requests;
using DemoBackend.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DemoBackend.Services;

public interface IJWTService
{
    string GenerateToken(GenerateTokenRequest request);
    string GenerateRefreshTokenHash();
}

public class JWTService(IOptions<JWTSettings> options) : IJWTService
{
    public string GenerateToken(GenerateTokenRequest request)
    {
        string secretKey = options.Value.SecretKey;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, request.Id),
                new Claim("email", request.Email),
                new Claim("username", request.DisplayName),
                new Claim("refreshTokenFamily", request.RefreshTokenFamilyId)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = options.Value.Issuer,
            Audience = options.Value.Audience
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshTokenHash()
    {
        return Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(options.Value.RefreshTokenSize));
    }
}