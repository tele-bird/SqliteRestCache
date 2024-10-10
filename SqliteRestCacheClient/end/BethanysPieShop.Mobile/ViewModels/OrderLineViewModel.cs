using BethanysPieShop.Mobile.Services.Interfaces;

namespace BethanysPieShop.Mobile.ViewModels;

public class OrderLineViewModel
{
    public long Id { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public PieViewModel Pie { get; set; } = default!;
}