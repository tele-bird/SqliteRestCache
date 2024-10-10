using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Services.Interfaces;

public interface IOrderService
{
    Task<Order?> GetOrderById(long id);
    
    Task<List<Order>> GetOrdersForUser(string userUuid);
    
    void AddOrder(Order orderEntity, string userUuid);
    
    Task SaveAsync();
}