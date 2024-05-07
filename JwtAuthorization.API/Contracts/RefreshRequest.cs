using System.ComponentModel.DataAnnotations;

namespace JwtAuthorization.API.Contracts
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}