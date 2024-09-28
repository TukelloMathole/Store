using UserService.DTOs;

namespace UserService.Services
{
    public interface IUserAuthenticationService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<string> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}
