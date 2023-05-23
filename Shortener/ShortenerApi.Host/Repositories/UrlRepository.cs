using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Dto;
using UrlShortener.Models.Enteties;
using UrlShortenerApi.Host.Data;
using UrlShortenerApi.Host.Repositories.Interfaces;

namespace UrlShortenerApi.Host.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ApplicationDbContext _context;

        public UrlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Url> GetUrlByOriginalStringAsync(string originalUrl)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
        }

        public async Task AddUrl(Url url)
        {
            await _context.Urls.AddAsync(url);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUrl(Url url)
        {
            _context.Urls.Remove(url);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Url>> GetAllAsync()
        {
            return await _context.Urls.ToListAsync();
        }

        public async Task<Url> CreateUrl(Url url)
        {
            _context.Urls.Add(url);
            await _context.SaveChangesAsync();
            return url;
        }

        public async Task<Url> GetUrlAsync(UrlDto request)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.Id == request.Id);
        }

        public async Task<string> GetLongUrlByShortAsync(string shortCode)
        {
            var url = await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
            return url?.OriginalUrl;
        }

        public async Task<Url> GetUrlByIdAsync(Guid id)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
