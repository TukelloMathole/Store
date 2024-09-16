using System.Threading.Tasks;
using MyApp.DTOs;
using MyApp.Models;

namespace MyApp.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<RefreshTokenDto> RefreshTokenAsync(string refreshToken);
    }
}
