using Microsoft.EntityFrameworkCore;
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
            var aboutInfo = await _context.About.SingleAsync();
            return aboutInfo.Content;
        }

        public async Task<bool> UpdateAboutInfoAsync(string newInfo)
        {
            var aboutInfo = await _context.About.SingleAsync();
            aboutInfo.Content = newInfo;
            _context.About.Update(aboutInfo);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }
    }
}
