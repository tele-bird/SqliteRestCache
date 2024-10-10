using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Services.Interfaces;

public interface IOrderService
{
    Task<long> CreateOrder(OrderCreateModel orderCreateModel);
    
    Task<OrderDetailModel?> GetOrderById(int id);
    
    Task<List<OrderModel>> GetOrderHistory();
}