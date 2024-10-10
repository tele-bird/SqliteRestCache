using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;

namespace BethanysPieShop.Mobile.ViewModels;

public class PieViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public decimal Price { get; set; }

    public string? ImageThumbnailUrl { get; set; }

    public int CategoryId { get; set; }
}