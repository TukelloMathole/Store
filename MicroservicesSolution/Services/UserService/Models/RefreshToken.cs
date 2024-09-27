using Microsoft.AspNetCore.Identity;

namespace UserService.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public bool IsRevoked { get; set; }
    }
}
