namespace BethanysPieShop.Mobile.Models;

public class OrderDetailModel
{
    public int Id { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public decimal OrderTotal { get; set; }

    public List<OrderLineModel> OrderLines { get; set; } = new();

    public ShippingInformationModel ShippingInformation { get; set; } = new();
    
    public long TrackAndTraceCode { get; set; }
}