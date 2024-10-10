using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Services.Interfaces;

public interface IShoppingCartService
{
    Task AddItem(ShoppingCartItemModel shoppingCartItem);

    Task ClearShoppingCart();
    
    Task Delete(int itemId);
    
    Task<ShoppingCartModel> GetShoppingCart();

    Task Update(ShoppingCartItemModel item);
}