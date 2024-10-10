using BethanysPieShop.Mobile.Models;
using SQLite;

namespace BethanysPieShop.Mobile.Data;


[Table("ShoppingCartItems")]
public class ShoppingCartItem 
{
    [PrimaryKey]
    [AutoIncrement]
    public int ItemId { get; set; }

    public long? CartId { get; set; }

    public ActionTypeEnum ActionId { get; set; }
    public string? ImageThumbnailUrl { get; set; }
    public int Id { get; set; }
    public string? Name { get; set; }
    public int PieId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public enum ActionTypeEnum
{
    None,
    Added,
    Updated,
    Deleted
}