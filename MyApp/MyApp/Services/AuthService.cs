using Microsoft.AspNetCore.Identity;
using MyApp.DTOs;
using MyApp.Models;
using MyApp.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using MyApp.Helpers;
using MyApp.Repositories;

namespace MyApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly PasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, PasswordHasher passwordHasher, ITokenService tokenService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            // Check if the user already exists
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            // Create the user
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email // Identity requires a UserName field
            };

            // Create user and hash password
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User registration failed: {errors}");
            }

            // Optionally assign the default role (e.g., "User")
            await _userManager.AddToRoleAsync(user, "User");

            // Return a success message
            return "User registered successfully";
        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            // Generate tokens
            var token = _tokenService.GenerateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Save refresh token
            await _refreshTokenRepository.AddRefreshTokenAsync(new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddMonths(1), // Example expiry
                User = user
            });

            return new LoginResponseDto
            {
                IsLoggedIn = true,
                Token = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<RefreshTokenDto> RefreshTokenAsync(string refreshToken)
        {
            // Validate refresh token
            var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if (existingRefreshToken == null || existingRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            // Generate new tokens
            var user = await _userRepository.GetUserByIdAsync(existingRefreshToken.UserId);
            var newToken = _tokenService.GenerateJwtToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Update the old refresh token with a new one
            existingRefreshToken.Token = newRefreshToken;
            existingRefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(7); // Set new expiry date
            await _refreshTokenRepository.UpdateAsync(existingRefreshToken);

            // Return new tokens
            return new RefreshTokenDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}

