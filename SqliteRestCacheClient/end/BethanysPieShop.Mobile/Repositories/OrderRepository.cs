using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;

namespace BethanysPieShop.Mobile.Repositories;

public class OrderRepository : Repository, IOrderRepository
{
    private readonly IAuthRepository _authRepository;

    public OrderRepository(
        IHttpClientFactory httpClientFactory,
        IAuthRepository authRepository)
        : base(httpClientFactory)
    {
        _authRepository = authRepository;
    }

    public async Task<long> CreateOrder(OrderCreateModel orderCreateModel)
    {
        var authToken = await GetAuthorizationToken();
        var client = CreateHttpClient(authToken);

        var content = new StringContent(JsonSerializer.Serialize(orderCreateModel), Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync(
            "api/orders",
            content);

        response.EnsureSuccessStatusCode();

        var order = await response.Content.ReadFromJsonAsync<OrderModel>();
        return order!.Id;
    }

    public async Task<OrderDetailModel?> GetOrderById(int id)
    {
        var authToken = await GetAuthorizationToken();
        var client = CreateHttpClient(authToken);

        OrderDetailModel? order = await client.GetFromJsonAsync<OrderDetailModel>(
            $"api/orders/{id}");

        return order ?? new OrderDetailModel();
    }

    public async Task<List<OrderModel>> GetOrderHistory()
    {
        var authToken = await GetAuthorizationToken();
        var client = CreateHttpClient(authToken);

        var orders = await client.GetFromJsonAsync<List<OrderModel>>(
            "api/orders");

        return orders ?? new List<OrderModel>();
    }

    private async Task<AuthToken> GetAuthorizationToken()
    {
        var authToken = await _authRepository.GetAuthorizationToken();
        if (authToken is null)
        {
            throw new UnauthorizedAccessException();
        } // Not logged in or login expired

        return authToken;
    }
}
