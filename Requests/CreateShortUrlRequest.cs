namespace UrlShortener.Requests;

public class CreateShortUrlRequest
{
    public string OriginalUrl { get; set;} = string.Empty;

    public string? CustomShortCode { get; set;} 
}
