using BethanysPieShop.API.DBContexts;
using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.API.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly BethanysPieShopDbContext _context;

    public ShoppingCartRepository(BethanysPieShopDbContext context)
    {
        _context = context;
    }

    public Task<ShoppingCart?> GetById(long id)
        => _context.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.Pie)
            .FirstOrDefaultAsync(c => c.Id == id);

    public Task<ShoppingCart?> GetByUserId(string userId)
        => _context.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.Pie)
            .FirstOrDefaultAsync(c => c.UserUuid == userId);

    public Task SaveAsync()
        => _context.SaveChangesAsync();

    public void Delete(ShoppingCartItem item)
        => _context.ShoppingCartItems
            .Remove(item);

    public ShoppingCart AddCart(ShoppingCart shoppingCart)
    {
        _context.ShoppingCarts.Add(shoppingCart);
        return shoppingCart;
    }

    public void AddItem(ShoppingCartItem shoppingCartItem, long shoppingCartId)
    {
        shoppingCartItem.ShoppingCartId = shoppingCartId;
        _context.ShoppingCartItems.Add(shoppingCartItem);
    }
}