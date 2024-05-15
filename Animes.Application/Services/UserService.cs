using AnimeAPI.Infrastructure.Data;
using Animes.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(bool, string)> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user == null)
        {
            return (false, "Usuário não encontrado.");
        }

        // Verifique se a senha fornecida corresponde à senha armazenada no banco de dados
        if (user.Password != password)
        {
            return (false, "Senha incorreta.");
        }

        // Se a autenticação for bem-sucedida, retorne true
        return (true, "Autenticação bem-sucedida.");
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                return false; // Usuário já existe
            }

            await _userRepository.AddUserAsync(user);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }
}
