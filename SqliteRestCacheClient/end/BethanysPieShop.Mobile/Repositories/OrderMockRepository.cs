using AutoMapper;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;

namespace BethanysPieShop.Mobile.Repositories;

public class OrderMockRepository : IOrderRepository
{
    private readonly IMapper _mapper;
    private static List<OrderDetailModel> Orders { get; set;  } = new();

    public OrderMockRepository(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public Task<long> CreateOrder(OrderCreateModel orderCreateModel)
    {
        Random random = new Random();

        var orderLines = new List<OrderLineModel>();
        var i = 1;
        foreach (var orderLine in orderCreateModel.OrderLines)
        {
            orderLines.Add(new OrderLineModel
            {
                Id = i,
                Quantity = orderLine.Quantity,
                TotalPrice = orderLine.TotalPrice,
                UnitPrice = orderLine.UnitPrice,
                Pie = new PieModel
                {
                    Id = orderLine.PieId,
                    CategoryId = 1,
                    Name = "Mock pie",
                    Price = orderLine.UnitPrice
                }
            });
            
            i++;
        }

        var orderDetails = new OrderDetailModel
        {
            Id = Orders.Count + 1,
            OrderDate = DateTime.UtcNow,
            OrderTotal = orderCreateModel.OrderLines.Sum(ol => ol.TotalPrice),
            ShippingInformation = _mapper.Map<ShippingInformationModel>(orderCreateModel.ShippingInformation),
            TrackAndTraceCode = random.NextInt64(99999),
            OrderLines = orderLines
        };
        
        Orders.Add(orderDetails);

        return Task.FromResult((long)orderDetails.Id);
    }

    public Task<OrderDetailModel?> GetOrderById(int id)
    {
        var order = Orders.Find(o => o.Id == id);
        return Task.FromResult(order);
    }

    public Task<List<OrderModel>> GetOrderHistory()
    {
        var orders = Orders.Select(o => new OrderModel
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            OrderTotal = o.OrderTotal
        }).ToList();

        return Task.FromResult(orders);
    }
}