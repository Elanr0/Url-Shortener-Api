namespace UrlShortener.Requests;

public class CreateShortUrlRequest
{
    public string OriginalUrl { get; set;} = string.Empty;

    public int UserId {get; set;}

    public string? CustomShortCode { get; set;} 
}
