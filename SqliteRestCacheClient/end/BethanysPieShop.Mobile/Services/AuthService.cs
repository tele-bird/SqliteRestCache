using BethanysPieShop.Mobile.Repositories.Interfaces;
using BethanysPieShop.Mobile.Services.Interfaces;

namespace BethanysPieShop.Mobile.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public Task Login(string email, string password)
        => _authRepository.Login(email, password);

    public Task<bool> IsLoggedIn()
        => _authRepository.IsLoggedIn();

    public Task Register(string email, string password)
        => _authRepository.Register(email, password);

    public Task Logout()
        => _authRepository.Logout();

    public async Task<string?> GetUserEmail()
        => await _authRepository.GetUserEmail();
}