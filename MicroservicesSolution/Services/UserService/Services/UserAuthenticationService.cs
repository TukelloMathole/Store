using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Data;
using UserService.DTOs;
using UserService.Models;

namespace UserService.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserAuthenticationService> _logger;

        public UserAuthenticationService(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserAuthenticationService> logger)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            // Validate the input DTO
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto), "Login DTO cannot be null.");
            }

            if (string.IsNullOrEmpty(loginDto.Email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(loginDto.Email));
            }

            if (string.IsNullOrEmpty(loginDto.Password))
            {
                throw new ArgumentException("Password cannot be null or empty.", nameof(loginDto.Password));
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var role = userRoles.FirstOrDefault(); // Assuming the user has one role

                // Create claims
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName))),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer"))
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60")),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key"))),
                        SecurityAlgorithms.HmacSha256));

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                var refreshToken = GenerateJwtRefreshToken();
                await StoreRefreshTokenAsync(user.Id, refreshToken, DateTime.UtcNow.AddMonths(1));

                return new LoginResponseDto
                {
                    IsLoggedIn = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Role = role
                };
            }

            _logger.LogWarning("Invalid login attempt for email: {Email}", loginDto.Email);
            return new LoginResponseDto
            {
                IsLoggedIn = false,
                AccessToken = null,
                RefreshToken = null,
                Role = null
            };
        }
        private string GenerateJwtRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }
        private async Task StoreRefreshTokenAsync(string userId, string refreshToken, DateTime expiration)
        {
            var token = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                Expiration = expiration,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }
        public async Task<string> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            // Validate the incoming refresh token
            var existingRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshTokenDto.RefreshToken && !rt.IsRevoked);

            if (existingRefreshToken == null || existingRefreshToken.Expiration <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }

            // Retrieve the user associated with the refresh token
            var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId);
            if (user == null)
            {
                throw new SecurityTokenException("Invalid refresh token user.");
            }

            // Generate a new JWT token
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName))),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer"))
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60")),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key"))),
                    SecurityAlgorithms.HmacSha256));

            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Generate a new refresh token
            var newRefreshToken = GenerateJwtRefreshToken();
            await StoreRefreshTokenAsync(user.Id, newRefreshToken, DateTime.UtcNow.AddMonths(1));

            return newAccessToken;
        }
    }
}
