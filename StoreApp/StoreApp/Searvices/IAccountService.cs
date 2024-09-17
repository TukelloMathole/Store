using StoreApp.DTOs;
using System.Threading.Tasks;
using StoreApp.Models;
using Microsoft.AspNetCore.Identity;

namespace StoreApp.Searvices
{
    public interface IAccountService
    {
        Task<bool> RegisterUserAsync(RegisterDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<IdentityUser?> FindUserByEmailAsync(string email); 
        Task<bool> AssignRoleAsync(IdentityUser user, string roleName);
    }
}
