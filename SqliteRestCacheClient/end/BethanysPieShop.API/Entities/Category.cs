namespace BethanysPieShop.API.Entities;

public class Category
{
    public Category(int id, string label)
    {
        Id = id;
        Label = label;
    }

    public int Id { get; set; }

    public string Label { get; set; }

    public ICollection<Pie> Pies = new List<Pie>();
}