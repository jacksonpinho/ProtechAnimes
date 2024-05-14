using AnimeAPI.Infrastructure.Data;
using Animes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AnimeDbContext _context;
    private readonly IUserService _userService;

    public UserController(AnimeDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    // POST: api/User
    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo usuário.")]
    [SwaggerResponse(201, "Retorna o novo usuário criado.", typeof(User))]
    [SwaggerResponse(400, "Se o usuário já existir.")]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        // Verifica se o usuário já existe no banco de dados
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            return BadRequest("O usuário já está cadastrado.");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obtém um usuário por ID.")]
    [SwaggerResponse(200, "Retorna o usuário correspondente ao ID especificado.", typeof(User))]
    [SwaggerResponse(404, "Se não houver usuário correspondente ao ID especificado.")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("O usuário não foi encontrado.");
        }

        return user;
    }
}
