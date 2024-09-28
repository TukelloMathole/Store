using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.DTOs;
using UserService.Services;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly ILogger<UserAuthenticationController> _logger;

        public UserAuthenticationController(IUserAuthenticationService userAuthenticationService, ILogger<UserAuthenticationController> logger)
        {
            _userAuthenticationService = userAuthenticationService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login attempt.");
                return BadRequest(new { message = "Invalid login data" });
            }

            var loginResponse = await _userAuthenticationService.LoginAsync(loginDto);

            if (!loginResponse.IsLoggedIn)
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", loginDto.Email);
                return Unauthorized(new { message = "Invalid credentials" });
            }

            return Ok(loginResponse);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid refresh token data.");
                return BadRequest(new { message = "Invalid refresh token data" });
            }

            try
            {
                var newAccessToken = await _userAuthenticationService.RefreshTokenAsync(refreshTokenDto);
                return Ok(new { AccessToken = newAccessToken });
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(ex.Message);
                return Unauthorized(new { message = "Invalid refresh token" });
            }
        }
    }
}
