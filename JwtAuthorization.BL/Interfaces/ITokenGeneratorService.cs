using System.Security.Claims;

namespace JwtAuthorization.BL.Interfaces
{
    public interface ITokenGeneratorService
    {
        public string GenerateToken(string secretKey, string issuer, string audience, DateTime expirationTime,
            IEnumerable<Claim> claims = null);
    }
}