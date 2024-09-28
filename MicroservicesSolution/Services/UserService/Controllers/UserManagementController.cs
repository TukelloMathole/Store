using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.DTOs;
using UserService.Models;
using UserService.Services;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly ILogger<UserManagementController> _logger;

        public UserManagementController(IUserManagementService userManagementService, ILogger<UserManagementController> logger)
        {
            _userManagementService = userManagementService;
            _logger = logger;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagementService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("user/email/{email}")]
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            var user = await _userManagementService.FindUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(user);
        }

        [HttpGet("users/role/{roleName}")]
        public async Task<IActionResult> GetUsersWithRole(string roleName)
        {
            var users = await _userManagementService.GetUsersWithRoleAsync(roleName);
            return Ok(users);
        }

        [HttpPut("user/update")]
        public async Task<IActionResult> UpdateUser([FromBody] IdentityUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userManagementService.UpdateUserAsync(user);
            if (!result)
            {
                return NotFound(new { message = "User not found or update failed" });
            }

            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userManagementService.DeleteUserAsync(userId);
            if (!result)
            {
                return NotFound(new { message = "User not found or deletion failed" });
            }

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
