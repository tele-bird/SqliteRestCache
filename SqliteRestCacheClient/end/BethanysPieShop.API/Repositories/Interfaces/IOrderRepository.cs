using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetOrderById(long id);
    
    Task<List<Order>> GetOrdersForUser(string userUuid);
    
    void AddOrder(Order orderEntity);
    
    Task SaveAsync();
}