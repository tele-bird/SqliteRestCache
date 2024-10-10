using BethanysPieShop.API.DBContexts;
using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.API.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly BethanysPieShopDbContext _context;

    public OrderRepository(BethanysPieShopDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Order?> GetOrderById(long id) =>
        await _context.Orders
            .Include(o => o.OrderLines)
            .ThenInclude(ol => ol.Pie)
            .Include(o => o.ShippingInformation)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<List<Order>> GetOrdersForUser(string userUuid)
        => await _context.Orders
            .Where(o => o.UserId == userUuid)
            .OrderByDescending(o => o.Id)
            .ToListAsync();

    public void AddOrder(Order orderEntity) 
        => _context.Orders
            .Add(orderEntity);

    public async Task SaveAsync() 
        => await _context.SaveChangesAsync();
}