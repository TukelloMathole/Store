using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using StoreApp.Models;
using StoreApp.DTOs;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using Microsoft.Extensions.Logging;
using StoreApp.Searvices;

namespace StoreApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<IdentityUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }

        public async Task<IdentityUser?> FindUserByEmailAsync(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding user by email: {Email}", email);
                throw;
            }
        }

        public async Task<bool> RegisterUserAsync(RegisterDto registerDto)
        {
            var user = new IdentityUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                await _userManager.AddToRoleAsync(user, "User");
                return true;
            }

            _logger.LogWarning("User registration failed for email: {Email}. Errors: {Errors}", registerDto.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return false;
        }

        public async Task<bool> AssignRoleAsync(IdentityUser user, string roleName)
        {
            if (user == null || string.IsNullOrEmpty(roleName))
            {
                _logger.LogWarning("Invalid parameters: User or RoleName is null or empty.");
                return false;
            }

            try
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to create role: {RoleName}. Errors: {Errors}", roleName, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        return false;
                    }
                }

                var isInRole = await _userManager.IsInRoleAsync(user, roleName);
                if (isInRole)
                {
                    _logger.LogInformation("User is already in role: {RoleName}", roleName);
                    return true;
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Failed to add user to role: {RoleName}. Errors: {Errors}", roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception assigning role: {RoleName}", roleName);
                return false;
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var role = userRoles.FirstOrDefault(); // Assuming the user has one role

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]) // Issuer claim
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                        SecurityAlgorithms.HmacSha256));

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                var refreshToken = GenerateJwtRefreshToken();
                await StoreRefreshTokenAsync(user.Id, refreshToken, DateTime.Now.AddMonths(1));

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
    }
}
