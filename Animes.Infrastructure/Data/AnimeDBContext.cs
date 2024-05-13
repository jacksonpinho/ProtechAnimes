using Microsoft.EntityFrameworkCore;
using Animes.Domain.Entities;

namespace AnimeAPI.Infrastructure.Data
{
    public class AnimeDbContext : DbContext
    {
        public AnimeDbContext(DbContextOptions<AnimeDbContext> options) : base(options)
        {
        }

        public DbSet<Anime> Animes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aqui você pode configurar o modelo de dados, como chaves primárias, índices, relacionamentos, etc.
        }
    }
}
