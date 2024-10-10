namespace BethanysPieShop.API.DTO;

public class OrderLineDto
{
    public long Id { get; set; }
    public int PieId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public PieDto Pie { get; set; } = default!;
}