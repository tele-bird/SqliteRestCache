namespace BethanysPieShop.API.Entities;

public class Pie
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public string? ImageThumbnailUrl { get; set; }

    public string? ShortDescription { get; set; } 

    public string? LongDescription { get; set; } 

    public string? ImageUrl { get; set; } 

    public bool IsPieOfTheWeek { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; } = default!;

    public ICollection<OrderLine>? OrderLines { get; set; }

    public ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
}