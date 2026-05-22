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

    public ShortUrl CreateShortUrl(string originalUrl, int userId, string? customShortCode)
    {
        var ShortCode = string.IsNullOrEmpty(customShortCode) ? GenerateShortCode() : customShortCode;

        var shortCodeExists = _context.ShortUrls.Any(x => x.ShortCode == ShortCode);

        if (shortCodeExists)
        {
            throw new Exception("Bu özel kısa kod zaten kullanılıyor. Lütfen başka bir kod deneyin.");
        }

        var shortUrl = new ShortUrl
        {
            OriginalUrl = originalUrl,
            ShortCode = string.IsNullOrEmpty(customShortCode) ? GenerateShortCode() : customShortCode,
            CreatedAt = DateTime.Now,
            UserId = userId
            
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

    public List<ShortUrl> GetUserUrls(int userId)
    {
        return _context.ShortUrls.Where(x=> x.UserId == userId).ToList();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
