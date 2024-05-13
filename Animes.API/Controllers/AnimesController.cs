// Interface do Usuário (Web API)
using Animes.API.Application.Services;
using Animes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Animes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimesController : ControllerBase
    {
        private readonly IAnimeService _animeService;

        public AnimesController(IAnimeService animeService)
        {
            _animeService = animeService;
        }

        [HttpGet]
        public IActionResult GetAnimes()
        {
            var animes = _animeService.GetAnimes();
            return Ok(animes);
        }

        [HttpGet("{id}")]
        public IActionResult GetAnime(int id)
        {
            var anime = _animeService.GetAnime(id);
            if (anime == null)
            {
                return NotFound();
            }
            return Ok(anime);
        }

        [HttpPost]
        public IActionResult AddAnime([FromBody] Anime anime)
        {
            _animeService.AddAnime(anime);
            return CreatedAtAction(nameof(GetAnime), new { id = anime.Id }, anime);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnime(int id, [FromBody] Anime anime)
        {
            _animeService.UpdateAnime(id, anime);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnime(int id)
        {
            _animeService.DeleteAnime(id);
            return NoContent();
        }
    }
}
