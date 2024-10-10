namespace BethanysPieShop.Mobile.Models;

public class OrderModel
{
    public long Id { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal OrderTotal { get; set; }
}