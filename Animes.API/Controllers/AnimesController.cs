using AnimeAPI.Infrastructure.Data;
using Animes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;

[Route("api/[controller]")]
[ApiController]
public class AnimesController : ControllerBase
{
    private readonly AnimeDbContext _context;

    public AnimesController(AnimeDbContext context)
    {
        _context = context;
    }

    // GET: api/Animes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Anime>>> GetAnimes(int pageIndex = 0, int pageSize = 10, string diretor = null, string nome = null, string palavraChave = null)
    {
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
    public async Task<ActionResult<Anime>> GetAnime(int id)
    {
        var anime = await _context.Animes.FindAsync(id);

        if (anime == null || anime.Excluido)
        {
            return NotFound();
        }

        return anime ;
    }

    // PUT: api/Animes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAnime(int id, Anime anime)
    {
        if (id != anime.Id)
        {
            return BadRequest();
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
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Animes
    [HttpPost]
    public async Task<ActionResult<Anime>> PostAnime(Anime anime)
    {
        _context.Animes.Add(anime);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAnime), new { id = anime.Id }, anime);
    }

    // DELETE: api/Animes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnime(int id)
    {
        var anime = await _context.Animes.FindAsync(id);
        if (anime == null)
        {
            return NotFound();
        }

        anime.Excluido = true; // Exclusão lógica
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AnimeExists(int id)
    {
        return _context.Animes.Any(e => e.Id == id);
    }
}
