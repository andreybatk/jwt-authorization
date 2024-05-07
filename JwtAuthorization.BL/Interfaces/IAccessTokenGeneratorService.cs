using JwtAuthorization.BL.Models;
using JwtAuthorization.DB.Entities;

namespace JwtAuthorization.BL.Interfaces
{
    public interface IAccessTokenGeneratorService
    {
        AccessToken GenerateToken(User user);
    }
}