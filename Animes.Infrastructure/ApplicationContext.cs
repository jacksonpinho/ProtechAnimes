using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animes.Infrastructure
{
    public class AnimeService : IAnimeService
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

        // Implementações dos demais métodos da interface IAnimeService

        //...
    }

    public class AnimeDbContext : DbContext
    {
        public DbSet<Anime> Animes { get; set; }

        public AnimeDbContext(DbContextOptions<AnimeDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações do modelo, se necessário
        }
    }
}
