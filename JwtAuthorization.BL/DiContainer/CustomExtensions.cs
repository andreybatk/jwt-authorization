using JwtAuthorization.BL.Interfaces;
using JwtAuthorization.BL.Services;
using JwtAuthorization.BL.Validators;
using JwtAuthorization.DB.Interfaces;
using JwtAuthorization.DB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JwtAuthorization.BL.DiContainer
{
    public static class CustomExtensions
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IAccessTokenGeneratorService, AccessTokenGeneratorService>();
            services.AddSingleton<IRefreshTokenGeneratorService, RefreshTokenGeneratorService>();
            services.AddSingleton<ITokenGeneratorService, TokenGeneratorService>();
            services.AddSingleton<RefreshTokenValidator>();

            services.AddScoped<IAuthenticatorService, AuthenticatorService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        }
    }
}