using UrlShortener.Models;

namespace UrlShortener.Services;
public class UrlShortenerService
{
    private readonly List<ShortUrl> _urls= new();

    public ShortUrl CreateShortUrl(string originalUrl)
    {
        
        var shortUrl = new ShortUrl
        {
            Id = _urls.Count + 1,
            OriginalUrl = originalUrl,
            ShortCode = GenerateShortCode(),
        };

        _urls.Add(shortUrl);
        return shortUrl;
    }

    public ShortUrl? GetShortUrl(string shortCode)
    {
       return _urls.FirstOrDefault(x => x. ShortCode == shortCode); 
    }

    public List<ShortUrl> GetAll()
    {
       return _urls;
    }

    private string GenerateShortCode()
    {
        return Guid.NewGuid().ToString()[..6];
    }

}
