namespace BethanysPieShop.API.Entities;

public class ShoppingCartItem
{
    public int Id { get; set; }

    //public string Name { get; set; } = default!;
    
    //public decimal Price { get; set; }
    
    //public string? ImageThumbnailUrl { get; set; }

    //public string? ShortDescription { get; set; }

    public int Quantity { get; set; }
    
    public int PieId { get; set; }
    
    public Pie Pie { get; set; } = default!;

    public long ShoppingCartId { get; set; }

    public ShoppingCart ShoppingCart { get; set; } = default!;
}