namespace BethanysPieShop.API.DTO;

public class PieDto
{ 
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? ImageThumbnailUrl { get; set; }

    public int CategoryId { get; set; }
    
}