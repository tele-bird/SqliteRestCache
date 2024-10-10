using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;
using BethanysPieShop.Mobile.Services.Interfaces;

namespace BethanysPieShop.Mobile.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository) 
        => _orderRepository = orderRepository;

    public Task<long> CreateOrder(OrderCreateModel orderCreateModel)
        => _orderRepository.CreateOrder(orderCreateModel);

    public Task<OrderDetailModel?> GetOrderById(int id)
        => _orderRepository.GetOrderById(id);

    public Task<List<OrderModel>> GetOrderHistory()
        => _orderRepository.GetOrderHistory();
}