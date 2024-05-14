using AnimeAPI.Infrastructure.Data;
using Animes.Application.Interfaces;
using Animes.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using Swashbuckle.AspNetCore.Annotations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AnimesController : ControllerBase
{
    private readonly AnimeDbContext _context;
    private readonly IAuthService _authService;

    public AnimesController(AnimeDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }


    // LISTAGEM
    [HttpGet("Listagem")]
    [SwaggerOperation(Summary = "Obtém uma lista paginada de animes com base em filtros opcionais.")]
    [SwaggerResponse(200, "Retorna a lista paginada de animes.", typeof(IEnumerable<Anime>))]
    public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes(int pageIndex = 0, int pageSize = 10, string diretor = null, string nome = null, string palavraChave = null)
    {
        // Adicione o middleware ao pipeline de solicitação HTTP
       
        var query = _context.Animes.Where(a => !a.Excluido);

        if (!string.IsNullOrEmpty(diretor))
            query = query.Where(a => a.Diretor.Contains(diretor));

        if (!string.IsNullOrEmpty(nome))
            query = query.Where(a => a.Nome.Contains(nome));

        if (!string.IsNullOrEmpty(palavraChave))
            query = query.Where(a => a.Resumo.Contains(palavraChave));

        var animes = await query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

        return animes;
    }

    // GET: api/Animes/5
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obtém um anime pelo seu ID.")]
    [SwaggerResponse(200, "Retorna o anime com o ID especificado.", typeof(Anime))]
    [SwaggerResponse(404, "O anime não foi encontrado.")]
    public async Task<ActionResult<Anime>> GetAnime(int id)
    {
        var anime = await _context.Animes.FindAsync(id);

        if (anime == null || anime.Excluido)
        {
            return NotFound("O anime não foi encontrado.");
        }

        return anime;
    }
    // PUT: api/Animes/5
    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Atualiza um anime existente.")]
    [SwaggerResponse(200, "O anime foi atualizado com sucesso.")]
    [SwaggerResponse(400, "Se o ID do anime não corresponde ao ID fornecido.")]
    [SwaggerResponse(404, "Se o anime não existe.")]
    public async Task<IActionResult> PutAnime(int id, Anime anime)
    {
        if (id != anime.Id)
        {
            return BadRequest("O ID fornecido não corresponde ao ID do anime.");
        }

        _context.Entry(anime).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AnimeExists(id))
            {
                return NotFound("O anime não foi encontrado.");
            }
            else
            {
                return StatusCode(500, "Ocorreu um erro ao atualizar o anime.");
            }
        }

        return Ok("O anime foi atualizado com sucesso.");
    }


    // POST: api/Animes
    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo anime.")]
    [SwaggerResponse(201, "Retorna o novo anime criado.", typeof(Anime))]
    public async Task<ActionResult<Anime>> PostAnime(Anime anime)
    {
        _context.Animes.Add(anime);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAnime), new { id = anime.Id }, anime);
    }

    // DELETE: api/Animes/5
    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Exclui um anime.")]
    [SwaggerResponse(200, "O anime foi excluído com sucesso.")]
    [SwaggerResponse(404, "Se o anime não existe.")]
    public async Task<IActionResult> DeleteAnime(int id)
    {
        var anime = await _context.Animes.FindAsync(id);
        if (anime == null)
        {
            return NotFound("O anime não foi encontrado.");
        }

        anime.Excluido = true; // Exclusão lógica
        await _context.SaveChangesAsync();

        return Ok("O anime foi excluído com sucesso.");
    }


    private bool AnimeExists(int id)
    {
        return _context.Animes.Any(e => e.Id == id);
    }

}
