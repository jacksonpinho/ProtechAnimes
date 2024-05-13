// Aplicação
using Animes.Domain.Entities;
using Animes.Infrastructure.Repositories;
using System.Collections.Generic;

namespace Animes.API.Application.Services
{
    public interface IAnimeService
    {
        IEnumerable<Anime> GetAnimes();
        Anime GetAnime(int id);
        void AddAnime(Anime anime);
        void UpdateAnime(int id, Anime anime);
        void DeleteAnime(int id);
    }

    public class AnimeService : IAnimeService
    {
        private readonly List<Anime> _animes = new List<Anime>();

        public IEnumerable<Anime> GetAnimes() => _animes;

        public Anime GetAnime(int id) => _animes.Find(a => a.Id == id);

        public void AddAnime(Anime anime) => _animes.Add(anime);

        public void UpdateAnime(int id, Anime anime)
        {
            var existingAnime = _animes.Find(a => a.Id == id);
            if (existingAnime != null)
            {
                existingAnime.Nome = anime.Nome;
                existingAnime.Resumo = anime.Resumo;
                existingAnime.Diretor = anime.Diretor;
            }
        }

        public void DeleteAnime(int id)
        {
            var existingAnime = _animes.Find(a => a.Id == id);
            if (existingAnime != null)
            {
                _animes.Remove(existingAnime);
            }
        }
    }
}
