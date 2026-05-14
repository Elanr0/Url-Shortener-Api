using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Services;
public class UrlShortenerService
{
    private readonly AppDbContext _context;

    public UrlShortenerService(AppDbContext context)
    {
        
      _context = context;

    }

    public ShortUrl CreateShortUrl(string originalUrl)
    {
        var shortUrl = new ShortUrl
        {
            OriginalUrl = originalUrl,
            ShortCode = GenerateShortCode(),
            CreatedAt = DateTime.Now
        };
        _context.ShortUrls.Add(shortUrl);
        _context.SaveChanges();
        return shortUrl;
    }

    public ShortUrl? GetShortUrl(string shortCode)
    {
        return _context.ShortUrls.FirstOrDefault(x => x.ShortCode == shortCode);
    }
    public List<ShortUrl> GetAll()
    {
        return _context.ShortUrls.ToList();
    }

    private string GenerateShortCode()
    {
        return Guid.NewGuid().ToString()[..6];
    }
}
