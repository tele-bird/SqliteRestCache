using System.Text;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;
using System.Text.Json;
using System.Net.Http.Json;
using BethanysPieShop.Mobile.Data;

namespace BethanysPieShop.Mobile.Repositories;

public class ShoppingCartRepository : Repository, IShoppingCartRepository
{
    private readonly IAuthRepository _authRepository;
    private long? CartId { get; set; }

    private readonly BethanysPieShopDatabase _database;

    private bool _isSyncing = false;

    public ShoppingCartRepository(
        IHttpClientFactory httpClientFactory,
        IAuthRepository authRepository,
        BethanysPieShopDatabase database)
        : base(httpClientFactory)
    {
        _authRepository = authRepository;
        _database = database;

        Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    public async Task AddItem(ShoppingCartItemModel shoppingCartItem)
    {
        HttpClient client = CreateHttpClient();

        if (CartId is null)
        {
            await CreateShoppingCart();
        }

        if (IsOnline)
        {
            var itemId = await PostItem(shoppingCartItem, client);
            shoppingCartItem.Id = itemId;

            await _database.AddItem(shoppingCartItem, CartId!.Value);
        }
        else
        {
            await _database.AddItem(shoppingCartItem, CartId, ActionTypeEnum.Added);
        }
    }



    public async Task ClearShoppingCart()
    {
        if (IsOnline)
        {
            var client = CreateHttpClient();

            var response = await client.DeleteAsync(
                $"api/shoppingCart/{CartId}/all");

            response.EnsureSuccessStatusCode();

            await _database.ClearShoppingCart();
        }
        else
        {
            await _database.ClearShoppingCart(ActionTypeEnum.Deleted);

            var items = await _database.GetShoppingCartItemsToSync(CartId);
            await DisplayItems(items);
        }
    }

    public async Task Delete(int id)
    {
        HttpClient client = CreateHttpClient();

        if (IsOnline)
        {
            await RemoveItem(id, client);
            await _database.DeleteItem(id);
        }
        else
        {
            await _database.DeleteItem(id, ActionTypeEnum.Deleted);
        }
    }

    public async Task<ShoppingCartModel?> GetShoppingCart()
    {
        if (CartId is null)
        {
            var item = await _database.GetLastShoppingCartItem();
            CartId = item?.CartId;

            if (CartId is null)
            {
                await CreateShoppingCart();
            }
        }

        var authToken = await _authRepository.GetAuthorizationToken();
        var client = CreateHttpClient(authToken);

        if (IsOnline)
        {
            ShoppingCartModel? shoppingCart = await client.GetFromJsonAsync<ShoppingCartModel>(
            $"api/shoppingCart/{CartId}");

            return shoppingCart ?? new ShoppingCartModel();
        }
        else
        {
            var items = await _database.GetShoppingCartItems(CartId);
            ShoppingCartModel shoppingCart = new ShoppingCartModel
            {
                Id = CartId,
                Items = items.Select(item => new ShoppingCartItemModel
                {
                    Id = item.Id,
                    PieId = item.PieId,
                    Quantity = item.Quantity,
                    Pie = new PieModel()
                    {
                        Id = item.PieId,
                        ImageThumbnailUrl = item.ImageThumbnailUrl,
                        Name = item.Name,
                        Price = item.Price
                    }
                })
                    .ToList()
            };

            return shoppingCart;
        }
    }

    public async Task UpdateItem(ShoppingCartItemModel shoppingCartItem)
    {
        var client = CreateHttpClient();

        if (IsOnline)
        {
            ArgumentNullException.ThrowIfNull(CartId);

            await Patchitem(shoppingCartItem, client);

            await _database.UpdateItem(shoppingCartItem);
        }
        else
        {
            await _database.UpdateItem(shoppingCartItem, ActionTypeEnum.Updated);

            
        }
    }

    private async Task DisplayItems(List<ShoppingCartItem>? items)
    {
        if (items is not null && items.Count > 0)
        {
            await Shell.Current.DisplayAlert("", JsonSerializer.Serialize(items), "OK");
        }
    }

    private async Task CreateShoppingCart()
    {
        var client = CreateHttpClient();

        CartId = await _database.GetShoppingCartId();

        if (CartId == null && IsOnline)
        {
            var response = await client.PostAsync(
                $"api/shoppingCart", null);

            response.EnsureSuccessStatusCode();

            CartId = await response.Content.ReadFromJsonAsync<long>();
        }
    }

    private async Task<int> PostItem(ShoppingCartItemModel shoppingCartItem, HttpClient client)
    {
        var content = new StringContent(JsonSerializer.Serialize(shoppingCartItem), Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync(
            $"api/shoppingCart/{CartId}/add",
            content);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<int>();
    }

    private async Task Patchitem(ShoppingCartItemModel shoppingCartItem, HttpClient client)
    {
        var content = new StringContent(JsonSerializer.Serialize(shoppingCartItem), Encoding.UTF8,
            "application/json");

        var response = await client.PatchAsync(
            $"api/shoppingCart/{CartId}/item/{shoppingCartItem.Id}",
            content);

        response.EnsureSuccessStatusCode();
    }

    private async Task RemoveItem(int id, HttpClient client)
    {
        var response = await client.DeleteAsync(
            $"api/shoppingCart/{CartId}/item/{id}");

        response.EnsureSuccessStatusCode();

        await _database.DeleteItem(id);
    }

    private async void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess == NetworkAccess.Internet && _isSyncing == false)
        {
            _isSyncing = true;

            var client = CreateHttpClient();

            var items = await _database.GetShoppingCartItemsToSync(CartId);
            await DisplayItems(items);

            if (items.Count > 0 && items.First().CartId is null)
            {
                await GetShoppingCart();
                foreach (var item in items)
                {
                    item.CartId = CartId;
                }
            }

            foreach (var item in items)
            {
                ShoppingCartItemModel itemModel = new ShoppingCartItemModel
                {
                    PieId = item.PieId,
                    Quantity = item.Quantity,
                };

                if (item.ActionId == ActionTypeEnum.Added)
                {
                    var itemId = await PostItem(itemModel, client);

                    itemModel.Id = itemId;
                    
                    await _database.UpdateItem(itemModel);
                }

                if (item.ActionId == ActionTypeEnum.Updated)
                {
                    itemModel.Id = item.Id;

                    await Patchitem(itemModel, client);

                    await _database.UpdateItem(itemModel);
                }

                if (item.ActionId == ActionTypeEnum.Deleted)
                {
                    await RemoveItem(item.Id, client);
                }
            }

            items = await _database.GetShoppingCartItemsToSync(CartId);
            await DisplayItems(items);

            _isSyncing = false;
        }

        if(e.NetworkAccess == NetworkAccess.None)
        {
            _isSyncing = false;
        }
    }
}
