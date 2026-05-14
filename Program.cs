using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=urlshortener.db");
});

builder.Services.AddScoped<UrlShortenerService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () =>
{
    return "UrlShortener API çalışıyor.";
});

app.MapPost("/shorten", (CreateShortUrlRequest request, UrlShortenerService service) =>
{
    if (string.IsNullOrWhiteSpace(request.OriginalUrl))
    {
        return Results.BadRequest("URL boş olamaz.");
    }

    if (!Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
    {
        return Results.BadRequest("Geçerli bir URL gir.");
    }

    var shortUrl = service.CreateShortUrl(request.OriginalUrl);

    return Results.Ok(new
    {
        originalUrl = shortUrl.OriginalUrl,
        shortCode = shortUrl.ShortCode,
        shortUrl = $"http://localhost:5000/{shortUrl.ShortCode}"
    });
});

app.MapGet("/{shortCode}", (string shortCode, UrlShortenerService service) =>
{
    var shortUrl = service.GetShortUrl(shortCode);

    if (shortUrl is null)
    {
        return Results.NotFound("Böyle bir kisa link bulunamadi.");
    }

    return Results.Redirect(shortUrl.OriginalUrl);
});

app.MapGet("/urls/all", (UrlShortenerService service) =>
{
    return Results.Ok(service.GetAll());
});

app.Run("http://localhost:5000");