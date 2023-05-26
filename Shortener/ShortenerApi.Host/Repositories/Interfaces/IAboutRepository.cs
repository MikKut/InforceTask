namespace UrlShortenerApi.Host.Repositories.Interfaces
{
    public interface IAboutRepository
    {
        Task<string> GetAboutInfoAsync();
        Task<bool> UpdateAboutInfoAsync(string newInfo);
    }
}