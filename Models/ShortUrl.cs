namespace UrlShortener.Models;

public class ShortUrl
{
    public int Id { get; set;}

    public string OriginalUrl { get; set;} = string.Empty;

    public string ShortCode { get; set;} = string.Empty;

    public DateTime CreatedAt { get; set;} = DateTime.Now;

}

