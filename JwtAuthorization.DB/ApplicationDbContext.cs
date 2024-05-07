using JwtAuthorization.DB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthorization.DB
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        // dotnet ef migrations add Init --project JwtAuthorization.DB --startup-project JwtAuthorization.API
        // dotnet ef database update --project JwtAuthorization.DB --startup-project JwtAuthorization.API
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}