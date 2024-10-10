using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;
using BethanysPieShop.Mobile.Services.Interfaces;

namespace BethanysPieShop.Mobile.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly IShoppingCartRepository _shoppingCartRepository;

    public ShoppingCartService(
        IShoppingCartRepository shoppingCartRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }

    public Task AddItem(ShoppingCartItemModel shoppingCartItem)
        => _shoppingCartRepository.AddItem(shoppingCartItem);

    public Task ClearShoppingCart()
        => _shoppingCartRepository.ClearShoppingCart();

    public Task Delete(int id)
        => _shoppingCartRepository.Delete(id);

    public Task<ShoppingCartModel> GetShoppingCart()
        => _shoppingCartRepository.GetShoppingCart();

    public Task Update(ShoppingCartItemModel item)
        => _shoppingCartRepository.UpdateItem(item);
}