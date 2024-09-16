using MyApp.Models;

namespace MyApp.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(int userId);
    }

}
