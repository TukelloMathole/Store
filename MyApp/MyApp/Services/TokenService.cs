using Microsoft.IdentityModel.Tokens;
using MyApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtKey;
        private readonly int _jwtExpiryMinutes;

        public TokenService(IConfiguration configuration)
        {
            _jwtKey = configuration["Jwt:Key"];
            _jwtExpiryMinutes = int.Parse(configuration["Jwt:ExpiryMinutes"]);
        }

        public string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            // Generate a secure random refresh token
            using var rng = new RNGCryptoServiceProvider();
            var randomBytes = new byte[32];
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
