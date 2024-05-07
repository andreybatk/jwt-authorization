using JwtAuthorization.BL.Interfaces;
using JwtAuthorization.BL.Models;
using JwtAuthorization.BL.Responses;
using JwtAuthorization.DB.Entities;
using JwtAuthorization.DB.Interfaces;

namespace JwtAuthorization.BL.Services
{
    public class AuthenticatorService : IAuthenticatorService
    {
        private readonly IAccessTokenGeneratorService _accessTokenGenerator;
        private readonly IRefreshTokenGeneratorService _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthenticatorService(IAccessTokenGeneratorService accessTokenGenerator, IRefreshTokenGeneratorService refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticatedUserResponse> AuthenticateAsync(User user)
        {
            AccessToken accessToken = _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                UserId = user.Id
            };
            await _refreshTokenRepository.Create(refreshTokenDTO);

            return new AuthenticatedUserResponse()
            {
                AccessToken = accessToken.Value,
                AccessTokenExpirationTime = accessToken.ExpirationTime,
                RefreshToken = refreshToken
            };
        }
    }
}