using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;

namespace BethanysPieShop.Mobile.Repositories;
public class AuthRepository : Repository, IAuthRepository
{
    private static AuthToken? AuthToken { get; set; }

    private const string UserEmailKey = "user_email";
    private const string AuthTokenKey = "auth_token";

    public AuthRepository(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }

    public async Task<AuthToken?> GetAuthorizationToken()
    {
        if (AuthToken == null)
        {
            var token = await SecureStorage.Default.GetAsync(AuthTokenKey);
            if (token is not null)
            {
                AuthToken = JsonSerializer.Deserialize<AuthToken>(token);
            }
        }

        if (AuthToken?.IsExpired ?? true)
        {
            AuthToken = null;
            SecureStorage.Default.Remove(AuthTokenKey);
        }

        return AuthToken;
    }

    public async Task<string> GetUserEmail()
    {
        var authToken = await GetAuthorizationToken();
        if (authToken is null)
        {
            throw new UnauthorizedAccessException();
        } // Not logged in or login expired

        if (await IsLoggedIn())
        {
            bool hasKey = Preferences.Default.ContainsKey(UserEmailKey);

            if (hasKey)
            {
                string userEmail = Preferences.Default.Get(UserEmailKey, "Unknown");
                return userEmail;
            }
        }

        var client = CreateHttpClient(authToken);
        var response = await client.GetAsync("useremail");

        string email = await response.Content.ReadFromJsonAsync<string>() ?? String.Empty;
        Preferences.Default.Set(UserEmailKey, email);
        return email;
    }

    public async Task<bool> IsLoggedIn()
    {
        var authToken = await GetAuthorizationToken();
        return authToken is not null && !authToken.IsExpired;
    }

    public async Task Login(string email, string password)
    {
        var client = CreateHttpClient();

        var loginModel = new LoginRequest(email, password);
        var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync(
            "login",
            content);

        if (response.IsSuccessStatusCode)
        {
            var authResponse = await response.Content.ReadFromJsonAsync<AuthToken>();
            if (authResponse != null)
            {
                authResponse.ExpirationDateTime = DateTime.UtcNow.AddSeconds(authResponse.ExpiresIn);
                AuthToken = new AuthToken
                {
                    AccessToken = authResponse.AccessToken,
                    ExpirationDateTime = authResponse.ExpirationDateTime,
                    ExpiresIn = authResponse.ExpiresIn,
                    RefreshToken = authResponse.RefreshToken,
                    TokenType = authResponse.TokenType
                };

                var tokenSerialized = JsonSerializer.Serialize(AuthToken);
                await SecureStorage.Default.SetAsync(AuthTokenKey, tokenSerialized);
            }
            else
            {
                throw new Exception("Login failed");
            }
        }
        else
        {
            Debug.WriteLine($"{response.StatusCode} : {response.ReasonPhrase}");
            throw new Exception(response.ReasonPhrase);
        }
    }

    public async Task Logout()
    {
        var authToken = await GetAuthorizationToken();
        if (authToken is null) return; // Not logged in or login expired

        var client = CreateHttpClient(authToken);

        try
        {
            await client.PostAsync(
                "logout", null);

            AuthToken = null;
            SecureStorage.Default.Remove(AuthTokenKey);
            Preferences.Default.Remove(UserEmailKey);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"{e.GetType().Name} : {e.Message}");
            throw;
        }
    }

    public async Task Register(string email, string password)
    {
        var client = CreateHttpClient();

        var loginModel = new LoginRequest(email, password);
        var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8,
            "application/json");

        await client.PostAsync(
             "register",
             content);
    }
}
