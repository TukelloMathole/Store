using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.DTOs;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRegistrationController : Controller
    {
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<UserRegistrationController> _logger;

        public UserRegistrationController(
            IUserRegistrationService userRegistrationService,
            UserManager<IdentityUser> userManager,
            ILogger<UserRegistrationController> logger)
        {
            _userRegistrationService = userRegistrationService;
            _userManager = userManager;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid registration attempt.");
                return BadRequest(new { message = "Invalid registration data" });
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User already exists with email: {Email}", model.Email);
                return BadRequest(new { message = "User already exists" });
            }

            var registrationSuccess = await _userRegistrationService.RegisterUserAsync(model);
            if (!registrationSuccess)
            {
                _logger.LogError("User registration failed for email: {Email}", model.Email);
                return BadRequest(new { message = "User registration failed" });
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogError("User not found after registration for email: {Email}", model.Email);
                return BadRequest(new { message = "User not found after registration" });
            }

            var roleAssignmentSuccess = await _userRegistrationService.AssignRoleAsync(user, "User");
            if (!roleAssignmentSuccess)
            {
                _logger.LogError("Failed to assign role to user: {UserId}", user.Id);
                return BadRequest(new { message = "Failed to assign role to user" });
            }

            _logger.LogInformation("User registered successfully with email: {Email}", model.Email);
            return Ok(new { message = "User registered and assigned role successfully" });
        }
    }
}
