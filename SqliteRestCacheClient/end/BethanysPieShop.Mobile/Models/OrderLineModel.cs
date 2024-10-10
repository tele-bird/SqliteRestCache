namespace BethanysPieShop.Mobile.Models;

public class OrderLineModel
{
    public long Id { get; set; }
    
    public int Quantity { get; set; }
    
    public decimal UnitPrice { get; set; }
    
    public decimal TotalPrice { get; set; }

    public PieModel Pie { get; set; } = new();
}