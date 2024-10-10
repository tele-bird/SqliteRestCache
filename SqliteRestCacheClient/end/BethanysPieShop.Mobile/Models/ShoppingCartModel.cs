using SQLite;
using SQLiteNetExtensions.Attributes;

namespace BethanysPieShop.Mobile.Models;

public class ShoppingCartModel
{
    public long? Id { get; set; }

    [OneToMany] public List<ShoppingCartItemModel>? Items { get; set; } = null;
}