using AnimeAPI.Infrastructure.Data;
using Animes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo usuário.")]
    [SwaggerResponse(201, "Retorna o novo usuário criado.", typeof(User))]
    [SwaggerResponse(400, "Se o usuário já existir.")]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        var success = await _userService.CreateUserAsync(user);
        if (!success)
        {
            return BadRequest("O usuário já está cadastrado.");
        }

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obtém um usuário por ID.")]
    [SwaggerResponse(200, "Retorna o usuário correspondente ao ID especificado.", typeof(User))]
    [SwaggerResponse(404, "Se não houver usuário correspondente ao ID especificado.")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound("O usuário não foi encontrado.");
        }

        return user;
    }
}
