using EntitiesLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        Task<bool> UserExistsAsync(string email);
        Task CreateUserAsync(User user);
    }
}
