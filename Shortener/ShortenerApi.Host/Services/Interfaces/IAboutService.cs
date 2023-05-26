namespace UrlShortenerApi.Host.Services.Interfaces
{
    public interface IAboutService
    {
        Task<bool> CheckWhetherUserCanEdditAboutInfoAwait(Guid userId);
        Task<string> GetAboutInfoAwait();
        Task<bool> UpdateAboutInfoAwait(string newInfo);
    }
}