

namespace Animes.Application.Interfaces
{
    public interface IUserService
    {
        Task<(bool, string)> AuthenticateAsync(string username, string password);
        Task<bool> CreateUserAsync(User user);
    }
}
