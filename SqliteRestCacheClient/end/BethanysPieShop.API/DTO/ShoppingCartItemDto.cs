namespace BethanysPieShop.API.DTO;

public class ShoppingCartItemDto
{
    public int Id { get; set; }

    //public string Name { get; set; } = default!;
    
    //public decimal Price { get; set; }

    //public string? ImageThumbnailUrl { get; set; }

    //public string? ShortDescription { get; set; }

    public int Quantity { get; set; }

    public int PieId { get; set; }

    public PieDto Pie { get; set; }
}