using AnimeAPI.Infrastructure.Data;
using Animes.Application.Interfaces;
using Animes.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthenticateController : ControllerBase
{
    private readonly AnimeDbContext _context;
    private readonly IAuthService _authService;

    public AuthenticateController(AnimeDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpPost("Authenticate")]
    [AllowAnonymous] // Permitir acesso anônimo para esta ação
    [SwaggerOperation(Summary = "Autentica um usuário e retorna um token JWT.")]
    [SwaggerResponse(200, "Usuário autenticado com sucesso.", typeof(string))]
    [SwaggerResponse(401, "Credenciais inválidas.")]
    public async Task<ActionResult<string>> Authenticate([FromBody] Login login)
    {
        // Verifique as credenciais do usuário
        var token = await _authService.Authenticate(login.Username, login.Password);
        if (token == null)
            return Unauthorized("Credenciais inválidas.");

        // Armazene o token JWT em um cookie seguro
        Response.Cookies.Append("authToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        // Se as credenciais forem válidas, retorne o token JWT
        return Ok("Usuário autenticado com sucesso.");
    }
}