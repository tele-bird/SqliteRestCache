using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;

namespace BethanysPieShop.Mobile.Repositories;

public class AuthMockRepository : IAuthRepository
{
    private static AuthToken? AuthToken { get; set; }

    public async Task<AuthToken?> GetAuthorizationToken()
    {
        if (AuthToken?.IsExpired ?? false)
        {
            AuthToken = null;
        }

        return await Task.FromResult(AuthToken);
    }

    public Task<string> GetUserEmail()
    {
        return AuthToken is null
            ? throw new UnauthorizedAccessException()
            : Task.FromResult("lindsey@snowball.be");
    }

    public Task<bool> IsLoggedIn() => Task.FromResult(AuthToken is not null && !AuthToken.IsExpired);

    public Task Login(string email, string password)
    {
        AuthToken = new AuthToken
        {
            AccessToken = "accessToken",
            ExpirationDateTime = DateTime.UtcNow.AddDays(1),
            RefreshToken = "RefreshToken",
            TokenType = "type"
        };
        return Task.CompletedTask;
    }

    public Task Logout()
    {
        AuthToken = null;
        return Task.CompletedTask;
    }

    public Task Register(string email, string password)
        => Task.CompletedTask;
}