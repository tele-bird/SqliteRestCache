namespace BethanysPieShop.API.DTO;

public class PieDetailDto : PieDto
{
    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsPieOfTheWeek { get; set; }

    public CategoryDto Category { get; set; } = default!;
}