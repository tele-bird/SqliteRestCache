using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Repositories.Interfaces;

public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetById(long id);
    
    Task<ShoppingCart?> GetByUserId(string userId);
    
    Task SaveAsync();
    
    void Delete(ShoppingCartItem item);

    ShoppingCart AddCart(ShoppingCart shoppingCart);

    void AddItem(ShoppingCartItem shoppingCartItem, long shoppingCartId);
}