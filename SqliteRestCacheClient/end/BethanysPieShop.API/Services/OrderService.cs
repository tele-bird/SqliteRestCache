using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using BethanysPieShop.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BethanysPieShop.API.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    
    public OrderService(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<Order?> GetOrderById(long id)
        => _orderRepository.GetOrderById(id);

    public Task<List<Order>> GetOrdersForUser(string userUuid)
        => _orderRepository.GetOrdersForUser(userUuid);

    public void AddOrder(Order orderEntity, string userUuid)
    {
        Random random = new Random();
        
        orderEntity.OrderDate = DateTime.UtcNow;
        orderEntity.OrderTotal = orderEntity.OrderLines.Sum(ol => ol.TotalPrice);
        orderEntity.OrderStatus = OrderStatusEnum.Paid;
        orderEntity.UserId = userUuid;
        orderEntity.TrackAndTraceCode = random.NextInt64();

        _orderRepository.AddOrder(orderEntity);
    }

    public Task SaveAsync()
        => _orderRepository.SaveAsync();
}