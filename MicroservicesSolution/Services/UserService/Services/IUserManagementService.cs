using System.Threading.Tasks;
using UserService.Models;
using Microsoft.AspNetCore.Identity;
using UserService.DTOs;

namespace UserService.Services
{
    public interface IUserManagementService
    {
        Task<bool> RegisterUserAsync(RegisterDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<IdentityUser?> FindUserByEmailAsync(string email);
        Task<bool> AssignRoleAsync(IdentityUser user, string roleName);
    }
}
