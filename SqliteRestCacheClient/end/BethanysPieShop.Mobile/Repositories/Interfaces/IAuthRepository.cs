using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<AuthToken?> GetAuthorizationToken();

    Task<string> GetUserEmail();
    
    Task<bool> IsLoggedIn();
    
    Task Login(string email, string password);
    
    Task Logout();
    
    Task Register(string email, string password);
}