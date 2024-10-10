namespace BethanysPieShop.API.DTO;

public class OrderDto
{
    public long Id { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal OrderTotal { get; set; }

    public List<OrderLineDto> OrderLines { get; set; } = default!;

    public ShippingInformationDto ShippingInformation { get; set; } = default!;

    public long TrackAndTraceCode { get; set; }
    
    public string UserId { get; set; } = default!;
}