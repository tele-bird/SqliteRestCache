namespace BethanysPieShop.API.Entities;

public class ShoppingCart
{
    public long Id { get; set; }
    
    public string? UserUuid { get; set; }
    
    public List<ShoppingCartItem> Items { get; set; } = [];
}