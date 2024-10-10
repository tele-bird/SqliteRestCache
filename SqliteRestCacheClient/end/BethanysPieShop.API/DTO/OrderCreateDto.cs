namespace BethanysPieShop.API.DTO;

public class OrderCreateDto
{
    public List<OrderLineCreateDto> OrderLines { get; set; } = new();

    public ShippingInformationDto ShippingInformation { get; set; } = default!;
}