using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Requests;

namespace UrlShortener.Services;

public class AuthServices
{
    private readonly AppDbContext _context;
    public AuthServices(AppDbContext context)
    {
        _context = context;
    }

    public User Register(RegisterRequest request)
    {
        var userExists = _context.Users.Any(x => x.Email == request.Email || x.PhoneNumber == request.PhoneNumber);

        if (userExists)
        {
            throw new Exception("Bu email veya telefon numarası zaten kayıtlı.");
        }

        var user = new User
        {
            Email= request.Email,
            PhoneNumber = request.PhoneNumber,
            PasswordHash= request.Password,
            CreatedAt= DateTime.Now,
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return user;
    }

    public User Login(LoginRequest request)
    {
        var user = _context.Users.FirstOrDefault(x => 
        x.Email == request.EmailOrPhoneNumber ||
        x.PhoneNumber == request.EmailOrPhoneNumber);

        if(user is null)
        {
            throw new Exception("Kullanıcı bulunamadı.");
        }

        if (user.PasswordHash != request.Password)
        {
            throw new Exception("Şifre yanlış.");
        }

        return user;
        
    }

}
