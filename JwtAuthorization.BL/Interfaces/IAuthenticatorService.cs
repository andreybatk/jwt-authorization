using JwtAuthorization.BL.Responses;
using JwtAuthorization.DB.Entities;

namespace JwtAuthorization.BL.Interfaces
{
    public interface IAuthenticatorService
    {
        Task<AuthenticatedUserResponse> AuthenticateAsync(User user);
    }
}