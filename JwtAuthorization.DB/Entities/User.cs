using Microsoft.AspNetCore.Identity;

namespace JwtAuthorization.DB.Entities
{
    public class User : IdentityUser<Guid> { }
}