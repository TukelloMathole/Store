using Microsoft.AspNetCore.Identity;

namespace MyApp.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string UserName { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }

}
