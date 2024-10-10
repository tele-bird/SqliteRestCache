namespace BethanysPieShop.API.DTO;

public class OrderLineCreateDto
{
    public int PieId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}