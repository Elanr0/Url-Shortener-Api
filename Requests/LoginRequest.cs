namespace UrlShortener.Requests;

public class LoginRequest
{
   public string EmailOrPhoneNumber {get; set;} = string.Empty;
   public string Password {get; set;} = string.Empty; 
}
