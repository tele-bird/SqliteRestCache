namespace BethanysPieShop.Mobile.Models;

public class PieModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string? ImageThumbnailUrl { get; set; }

    public int CategoryId { get; set; }
}