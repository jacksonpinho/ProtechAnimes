using AnimeAPI.Infrastructure.Data;
using Animes.Application.Interfaces;
using Animes.Domain.Entities;
using Microsoft.EntityFrameworkCore;



namespace Animes.API.Application.Services
{

    public class AnimeService : IAnimeInterface
    {
        private readonly AnimeDbContext _context;

        public AnimeService(AnimeDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Anime>> GetAnimesAsync(int pageIndex, int pageSize, string diretor, string nome, string palavraChave)
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

        public async Task AddAnimeAsync(Anime anime)
        {
            _context.Animes.Add(anime);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAnimeAsync(int id)
        {
            var anime = await _context.Animes.FindAsync(id);
            if (anime != null)
            {
                anime.Excluido = true; // Exclusão lógica
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Anime> GetAnimeByIdAsync(int id)
        {
            return await _context.Animes.FindAsync(id);
        }

        public async Task UpdateAnimeAsync(Anime anime)
        {
            _context.Entry(anime).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }


}