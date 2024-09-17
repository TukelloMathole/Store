﻿using Microsoft.AspNetCore.Mvc;
using StoreApp.DTOs;
using StoreApp.Searvices;
using System.Threading.Tasks;
using StoreApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace StoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, UserManager<IdentityUser> userManager, ILogger<AccountController> logger)
        {
            _accountService = accountService;
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

            var registrationSuccess = await _accountService.RegisterUserAsync(model);

            if (!registrationSuccess)
            {
                _logger.LogError("User registration failed for email: {Email}", model.Email);
                return BadRequest(new { message = "User registration failed" });
            }

            var user = await _accountService.FindUserByEmailAsync(model.Email);
            if (user == null)
            {
                _logger.LogError("User not found after registration for email: {Email}", model.Email);
                return BadRequest(new { message = "User not found after registration" });
            }

            var roleAssignmentSuccess = await _accountService.AssignRoleAsync(user, "User");
            if (!roleAssignmentSuccess)
            {
                _logger.LogError("Failed to assign role to user: {UserId}", user.Id);
                return BadRequest(new { message = "Failed to assign role to user" });
            }

            _logger.LogInformation("User registered successfully with email: {Email}", model.Email);
            return Ok(new { message = "User registered and assigned role successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login attempt.");
                return BadRequest(new { message = "Invalid login data" });
            }

            var loginResponse = await _accountService.LoginAsync(loginDto);
            if (loginResponse.IsLoggedIn)
            {
                _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
                return Ok(loginResponse);
            }

            _logger.LogWarning("Unauthorized login attempt for email: {Email}", loginDto.Email);
            return Unauthorized(new { message = "Invalid login attempt" });
        }
    }
}
