namespace BethanysPieShop.API.DTO;

public class ShoppingCartDto
{
    public long Id { get; set; }
    
    public List<ShoppingCartItemDto>? Items { get; set; }
}