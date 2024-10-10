using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;

namespace BethanysPieShop.Mobile.Repositories;

public class ShoppingCartMockRepository : IShoppingCartRepository
{
    private static ShoppingCartModel ShoppingCart { get; set;  } = new();
    
    public Task AddItem(ShoppingCartItemModel shoppingCartItem)
    {
        ShoppingCart.Items.Add(shoppingCartItem);
        return Task.CompletedTask;
    }

    public Task ClearShoppingCart()
    {
        ShoppingCart.Items = new();
        return Task.CompletedTask;
    }

    public Task Delete(int id)
    {
        var item = ShoppingCart.Items.FirstOrDefault(i => i.Id == id);

        if (item is not null)
        {
            ShoppingCart.Items.Remove(item);
        }
        
        return Task.CompletedTask;
    }

    public Task<ShoppingCartModel> GetShoppingCart()
     => Task.FromResult(ShoppingCart);

    public Task UpdateItem(ShoppingCartItemModel item)
    {
        var shoppingCartItem = ShoppingCart.Items.Find(i => i.PieId == item.PieId);
        if (shoppingCartItem is not null)
        {
            shoppingCartItem.Quantity = item.Quantity;
        }

        return Task.CompletedTask;
    }
}
