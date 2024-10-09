using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Data;
using UserService.DTOs;

namespace UserService.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserManagementService> _logger;

        public UserManagementService(UserManager<IdentityUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, ILogger<UserManagementService> logger)
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

        public async Task<List<IdentityUser>> GetUsersWithRoleAsync(string roleName)
        {
            var users = await _userManager.Users.ToListAsync();
            var usersWithRole = new List<IdentityUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                    usersWithRole.Add(user);
                }
            }

            return usersWithRole;
        }

        public async Task<List<IdentityUser>> GetAllUsersAsync()
        {
            try
            {
                return await _userManager.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all users.");
                throw;
            }
        }

        public async Task<bool> UpdateUserAsync(IdentityUser user)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser == null)
                {
                    _logger.LogError("User not found: {UserId}", user.Id);
                    return false;
                }

                // Update the fields
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                // Add other fields to update as necessary

                var result = await _userManager.UpdateAsync(existingUser);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", user.Id);
                return false;
            }
        }


        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }

                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                return false;
            }
        }
    }
}
