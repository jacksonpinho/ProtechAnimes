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
    private readonly IAnimeService _animeService;

    public AnimesController(IAnimeService animeService)
    {
        _animeService = animeService;
    }

    [HttpGet("Listagem")]
    [SwaggerOperation(Summary = "Obtém uma lista paginada de animes com base em filtros opcionais.")]
    [SwaggerResponse(200, "Retorna a lista paginada de animes.", typeof(IEnumerable<Anime>))]
    public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes(int pageIndex = 0, int pageSize = 10, string diretor = null, string nome = null, string palavraChave = null)
    {
        var animes = await _animeService.GetAnimesAsync(pageIndex, pageSize, diretor, nome, palavraChave);
        return Ok(animes);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Obtém um anime pelo seu ID.")]
    [SwaggerResponse(200, "Retorna o anime com o ID especificado.", typeof(Anime))]
    [SwaggerResponse(404, "O anime não foi encontrado.")]
    public async Task<ActionResult<Anime>> GetAnime(int id)
    {
        var anime = await _animeService.GetAnimeByIdAsync(id);
        if (anime == null || anime.Excluido)
        {
            return NotFound("O anime não foi encontrado.");
        }
        return anime;
    }

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

        await _animeService.UpdateAnimeAsync(anime);
        return Ok("O anime foi atualizado com sucesso.");
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Cria um novo anime.")]
    [SwaggerResponse(201, "Retorna o novo anime criado.", typeof(Anime))]
    public async Task<ActionResult<Anime>> PostAnime(Anime anime)
    {
        await _animeService.AddAnimeAsync(anime);
        return CreatedAtAction(nameof(GetAnime), new { id = anime.Id }, anime);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Exclui um anime.")]
    [SwaggerResponse(200, "O anime foi excluído com sucesso.")]
    [SwaggerResponse(404, "Se o anime não existe.")]
    public async Task<IActionResult> DeleteAnime(int id)
    {
        await _animeService.DeleteAnimeAsync(id);
        return Ok("O anime foi excluído com sucesso.");
    }
}
