using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using BethanysPieShop.API.Services.Interfaces;

namespace BethanysPieShop.API.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;

    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }

    public Task<ShoppingCart?> GetById(long id)
        => _shoppingCartRepository.GetById(id);

    public async Task<ShoppingCart?> GetByUserId(string userId)
        => await _shoppingCartRepository.GetByUserId(userId) ?? await CreateShoppingCart(userId);

    public async Task AddItem(ShoppingCartItem shoppingCartItem, long id, string? userId = null)
    {
        var shoppingCart = await GetById(id) ?? new ShoppingCart();
        if (string.IsNullOrEmpty(shoppingCart.UserUuid) && !string.IsNullOrEmpty(userId))
        {
            shoppingCart.UserUuid = userId;
        }

        if (shoppingCart.Items.Any(i => i.PieId == shoppingCartItem.PieId))
        {
            ShoppingCartItem item = shoppingCart.Items.Find(i => i.PieId == shoppingCartItem.PieId)!;
            item.Quantity += shoppingCartItem.Quantity;
            await _shoppingCartRepository.SaveAsync();

            return;
        }

        _shoppingCartRepository.AddItem(shoppingCartItem, shoppingCart.Id);
    }

    public Task SaveAsync()
        => _shoppingCartRepository.SaveAsync();

    public async Task UpdateItemQuantity(long id, long itemId, int quantity, string? userId = null)
    {
        var shoppingCart = await GetById(id);
        ArgumentNullException.ThrowIfNull(shoppingCart);

        if (string.IsNullOrEmpty(shoppingCart.UserUuid) && !string.IsNullOrEmpty(userId))
        {
            shoppingCart.UserUuid = userId;
        }

        var item = shoppingCart.Items.Find(i => i.Id == itemId);
        if (item is not null)
        {
            item.Quantity = quantity;
        }
    }

    public async Task DeleteItem(long itemId, long id)
    {
        var shoppingCart = await GetById(id) ?? new ShoppingCart();

        var item = shoppingCart.Items.Find(i => i.Id == itemId);

        if (item is not null)
        {
            _shoppingCartRepository.Delete(item);
        }
    }

    public async Task<ShoppingCart> CreateShoppingCart(string? userId = null)
    {
        ShoppingCart? shoppingCart = null;
        if (userId is not null)
        {
            shoppingCart = await _shoppingCartRepository.GetByUserId(userId);
        }

        if (shoppingCart is null)
        {
            shoppingCart = new ShoppingCart
            {
                UserUuid = userId
            };

            shoppingCart = _shoppingCartRepository.AddCart(shoppingCart);
            await _shoppingCartRepository.SaveAsync();
        }

        return shoppingCart;
    }
}