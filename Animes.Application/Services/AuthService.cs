using Animes.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthService(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    public async Task<string> Authenticate(string username, string password)
    {
        // Autenticar o usuário
        var (isAuthenticated, message) = await _userService.AuthenticateAsync(username, password);

        // Verificar se a autenticação foi bem-sucedida
        if (!isAuthenticated)
        {
            // Se a autenticação falhar, retorne null ou uma mensagem de erro
            return null;
        }

        // Se a autenticação for bem-sucedida, gere e retorne o token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, username)
          
                // Você pode adicionar outras reivindicações conforme necessário
            }),
            Expires = DateTime.UtcNow.AddSeconds(10), // Tempo de expiração do token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }


}
