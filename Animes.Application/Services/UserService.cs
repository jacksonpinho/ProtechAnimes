using AnimeAPI.Infrastructure.Data;
using Animes.Application.Interfaces;
using Animes.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class UserService : IUserService
{
    private readonly AnimeDbContext _context;

    public UserService(AnimeDbContext context)
    {
        _context = context;
    }

    public async Task<(bool, string)> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

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
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
