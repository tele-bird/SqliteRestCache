using System.Net.Http.Headers;
using System.Text.Json;
using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Repositories;
public class Repository
{
    protected readonly IHttpClientFactory HttpClientFactory;
    private bool _isOnline = true;
    
    protected readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    
    protected Repository(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
    }

    public bool IsOnline
    {
        get
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            _isOnline = accessType == NetworkAccess.Internet;
            return _isOnline;
        }
    }
   

    protected HttpClient CreateHttpClient(AuthToken? authToken = null)
    {
        var client = HttpClientFactory.CreateClient(MauiProgram.BethanysPieShopApiClient);

        // Add authorization token;
        if (authToken is not null)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(authToken.TokenType, authToken.AccessToken);
        }

        return client;
    }
}
