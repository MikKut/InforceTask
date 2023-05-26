using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.Models.Enteties;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Host.Repositories.Interfaces;

namespace UrlShortenerApi.Host.Repositories
{
    public class AboutRepository : IAboutRepository
    {
        private readonly ApplicationDbContext _context;

        public AboutRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetAboutInfoAsync()
        {
            return await Task.FromResult(AboutInfo.Content);
        }

        public async Task<bool> UpdateAboutInfoAsync(string newInfo)
        {
            if (newInfo.IsNullOrEmpty())
            {
                return await Task.FromResult(false);
            }

            AboutInfo.Content = newInfo;
            return await Task.FromResult(true);
        }

        private static class AboutInfo
        {
            public static string Content = "Some about info";
        }
    }
}
