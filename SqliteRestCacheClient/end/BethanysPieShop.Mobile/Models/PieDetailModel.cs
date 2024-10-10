namespace BethanysPieShop.Mobile.Models;

public class PieDetailModel : PieModel
{
    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsPieOfTheWeek { get; set; }

    public CategoryModel? Category { get; set; }
}