using AnimeAPI.Infrastructure.Data;
using Animes.Application.Interfaces;
using Animes.Domain.Entities;
using Microsoft.EntityFrameworkCore;



namespace Animes.API.Application.Services
{

    public class AnimeService : IAnimeService
    {
        private readonly IAnimeRepository _animeRepository;

        public AnimeService(IAnimeRepository animeRepository)
        {
            _animeRepository = animeRepository;
        }

        public async Task<IEnumerable<Anime>> GetAnimesAsync(int pageIndex, int pageSize, string diretor, string nome, string palavraChave)
        {
            return await _animeRepository.GetAnimesAsync(pageIndex, pageSize, diretor, nome, palavraChave);
        }

        public async Task<Anime> GetAnimeByIdAsync(int id)
        {
            return await _animeRepository.GetAnimeByIdAsync(id);
        }

        public async Task AddAnimeAsync(Anime anime)
        {
            await _animeRepository.AddAnimeAsync(anime);
        }

        public async Task UpdateAnimeAsync(Anime anime)
        {
            await _animeRepository.UpdateAnimeAsync(anime);
        }

        public async Task DeleteAnimeAsync(int id)
        {
            await _animeRepository.DeleteAnimeAsync(id);
        }
    }



}