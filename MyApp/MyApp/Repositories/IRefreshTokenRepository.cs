using MyApp.Models;

namespace MyApp.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);

        Task<RefreshToken> GetByTokenAsync(string token);

        // Update an existing refresh token
        Task UpdateAsync(RefreshToken refreshToken);
    }
}
