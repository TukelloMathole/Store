using Microsoft.AspNetCore.Identity;
using UserService.DTOs;

namespace UserService.Services
{
    public interface IUserRegistrationService
    {
        Task<bool> RegisterUserAsync(RegisterDto registerDto);
        Task<bool> AssignRoleAsync(IdentityUser user, string roleName);
    }
}
