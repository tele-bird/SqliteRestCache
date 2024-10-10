using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Services.Interfaces;

public interface IShoppingCartService
{
    Task<ShoppingCart?> GetById(long id);

    Task<ShoppingCart?> GetByUserId(string userId);

    Task AddItem(ShoppingCartItem shoppingCartItem, long id, string? userId = null);

    Task SaveAsync();

    Task DeleteItem(long itemId, long id);

    Task<ShoppingCart> CreateShoppingCart(string? userId = null);

    Task UpdateItemQuantity(long id, long itemId, int quantity, string? userId = null);
}