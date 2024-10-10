namespace BethanysPieShop.API.Entities;

public class Order
{
    public long Id { get; set; }
    
    public DateTime OrderDate { get; set; }

    public decimal OrderTotal { get; set; }

    public List<OrderLine>? OrderLines { get; set; }

    public int ShippingInformationId { get; set; }

    public ShippingInformation? ShippingInformation { get; set; } 

    public OrderStatusEnum OrderStatus { get; set; }

    public string UserId { get; set; } = default!;
    
    public long TrackAndTraceCode { get; set; }
}