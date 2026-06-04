using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Requests;
using UrlShortener.Services;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT token gir. Örnek: Bearer eyJhbGciOi..."
    });

    options.AddSecurityRequirement(document =>new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key-for-url-shortener-project")),
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=urlshortener.db");
});

builder.Services.AddScoped<UrlShortenerService>();

builder.Services.AddScoped<AuthServices>();

builder.Services.AddScoped<TokenService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

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

app.MapPost("/auth/login", (LoginRequest request, AuthServices authServices, TokenService tokenService) =>
{
    var user = authServices.Login(request);

    var token = tokenService.CreateToken(user);

    return Results.Ok(new
    {
        message = "Giriş başarılı",
        token = token
    });
});

app.MapPost("/shorten", (CreateShortUrlRequest request, UrlShortenerService service, ClaimsPrincipal user) =>
{
    if (string.IsNullOrWhiteSpace(request.OriginalUrl))
    {
        return Results.BadRequest("URL boş olamaz.");
    }

    if (!Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
    {
        return Results.BadRequest("Geçerli bir URL gir.");
    }

    var userIdText = user.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userIdText is null)
    {
        return Results.Unauthorized();
    }
     
     var userId = int.Parse(userIdText);

    var shortUrl = service.CreateShortUrl(request.OriginalUrl, userId, request.CustomShortCode);

    return Results.Ok(new
    {
        originalUrl = shortUrl.OriginalUrl,
        shortCode = shortUrl.ShortCode,
        shortUrl = $"http://localhost:5000/{shortUrl.ShortCode}"
    });
}) .RequireAuthorization();

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