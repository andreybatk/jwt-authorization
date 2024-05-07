using JwtAuthorization.BL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthorization.BL.Services
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        public string GenerateToken(string secretKey, string issuer, string audience, DateTime expirationTime, IEnumerable<Claim> claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                DateTime.UtcNow,
                expirationTime,
                credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}