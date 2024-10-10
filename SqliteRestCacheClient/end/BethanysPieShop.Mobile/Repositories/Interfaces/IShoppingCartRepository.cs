using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Repositories.Interfaces;

public interface IShoppingCartRepository
{
    Task AddItem(ShoppingCartItemModel shoppingCartItem);
    
    Task ClearShoppingCart();
    
    Task Delete(int id);
    
    Task<ShoppingCartModel> GetShoppingCart();

    Task UpdateItem(ShoppingCartItemModel item);
}