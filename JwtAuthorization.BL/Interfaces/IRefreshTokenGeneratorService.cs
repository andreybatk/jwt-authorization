namespace JwtAuthorization.BL.Interfaces
{
    public interface IRefreshTokenGeneratorService
    {
        string GenerateToken();
    }
}