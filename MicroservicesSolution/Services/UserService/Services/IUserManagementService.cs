using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Models;
using Microsoft.AspNetCore.Identity;
using UserService.DTOs;

namespace UserService.Services
{
    public interface IUserManagementService
    {
        Task<IdentityUser?> FindUserByEmailAsync(string email);
        Task<List<IdentityUser>> GetUsersWithRoleAsync(string roleName);
        Task<List<IdentityUser>> GetAllUsersAsync(); 
        Task<bool> UpdateUserAsync(IdentityUser user); 
        Task<bool> DeleteUserAsync(string userId);
    }
}
