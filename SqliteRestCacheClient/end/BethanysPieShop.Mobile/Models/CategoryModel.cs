namespace BethanysPieShop.Mobile.Models;

public class CategoryModel
{
    public CategoryModel()
    {
    }

    public CategoryModel(int id, string label)
    {
        Id = id;
        Label = label;
    }

    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;
}