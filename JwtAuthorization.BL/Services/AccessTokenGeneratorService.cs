using JwtAuthorization.BL.Interfaces;
using JwtAuthorization.BL.Models;
using JwtAuthorization.DB.Entities;
using System.Security.Claims;

namespace JwtAuthorization.BL.Services
{
    public class AccessTokenGeneratorService : IAccessTokenGeneratorService
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly ITokenGeneratorService _tokenGeneratorService;

        public AccessTokenGeneratorService(AuthenticationConfiguration authenticationConfiguration, ITokenGeneratorService tokenGeneratorService)
        {
            _configuration = authenticationConfiguration;
            _tokenGeneratorService = tokenGeneratorService;
        }

        public AccessToken GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("ID", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpirationMinutes);
            string value = _tokenGeneratorService.GenerateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime,
                claims);

            return new AccessToken()
            {
                Value = value,
                ExpirationTime = expirationTime
            };
        }
    }
}
