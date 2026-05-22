using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Requests;
using UrlShortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=urlshortener.db");
});

builder.Services.AddScoped<UrlShortenerService>();

builder.Services.AddScoped<AuthServices>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () =>
{
    return "UrlShortener API çalışıyor.";
});

app.MapPost("/auth/register", (RegisterRequest request, AuthServices authServices) =>
{
        var user = authServices.Register(request);

        return Results.Ok(new
        {
            message = "Kayıt başarıyla oluşturuldu",
            userId = user.Id,
            email = user.Email,
            phoneNumber = user.PhoneNumber
        });
});

app.MapPost("/auth/login", (LoginRequest request, AuthServices authServices) =>
{
    var user = authServices.Login(request);

    return Results.Ok(new
    {
        message = "Giriş başarılı",
        userId = user.Id,
        email = user.Email,
        phoneNumber = user.PhoneNumber
    });
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

    var shortUrl = service.CreateShortUrl(request.OriginalUrl, request.UserId, request.CustomShortCode);

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

    shortUrl.ClickCount++;
    service.SaveChanges();

    return Results.Redirect(shortUrl.OriginalUrl);
});

app.MapGet("/urls/user/{userId}", (int userId, UrlShortenerService service) =>
{
    var userUrls = service.GetUserUrls(userId);


    return Results.Ok(userUrls);
});

app.MapGet("/urls/all", (UrlShortenerService service) =>
{
    return Results.Ok(service.GetAll());
});

app.Run("http://localhost:5000");