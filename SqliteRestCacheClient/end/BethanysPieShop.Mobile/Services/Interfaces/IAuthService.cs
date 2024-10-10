namespace BethanysPieShop.Mobile.Services.Interfaces;

public interface IAuthService
{
    Task<string?> GetUserEmail();
    
    Task<bool> IsLoggedIn();
    
    Task Login(string email, string password);
    
    Task Logout();
    
    Task Register(string email, string password);
}