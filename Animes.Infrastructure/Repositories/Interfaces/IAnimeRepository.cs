using Animes.Domain.Entities;

public interface IAnimeRepository
{
    Task<IEnumerable<Anime>> GetAnimesAsync(int pageIndex, int pageSize, string diretor, string nome, string palavraChave);
    Task<Anime> GetAnimeByIdAsync(int id);
    Task AddAnimeAsync(Anime anime);
    Task UpdateAnimeAsync(Anime anime);
    Task DeleteAnimeAsync(int id);
}