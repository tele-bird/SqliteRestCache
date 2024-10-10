using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<long> CreateOrder(OrderCreateModel orderCreateModel);
    
    Task<OrderDetailModel?> GetOrderById(int id);
    
    Task<List<OrderModel>> GetOrderHistory();
}