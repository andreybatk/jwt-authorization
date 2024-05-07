using JwtAuthorization.BL.Interfaces;
using JwtAuthorization.BL.Models;

namespace JwtAuthorization.BL.Services
{
    public class RefreshTokenGeneratorService : IRefreshTokenGeneratorService
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly ITokenGeneratorService _tokenGeneratorService;

        public RefreshTokenGeneratorService(AuthenticationConfiguration authenticationConfiguration, ITokenGeneratorService tokenGeneratorService)
        {
            _configuration = authenticationConfiguration;
            _tokenGeneratorService = tokenGeneratorService;
        }

        public string GenerateToken()
        {
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpirationMinutes);

            return _tokenGeneratorService.GenerateToken(
                _configuration.RefreshTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime);
        }
    }
}