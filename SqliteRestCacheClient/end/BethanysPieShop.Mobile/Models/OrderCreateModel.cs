namespace BethanysPieShop.Mobile.Models;

public class OrderCreateModel
{
    public List<OrderLineCreateModel> OrderLines { get; set; } = new();

    public ShippingInformationModel? ShippingInformation { get; set; }
}