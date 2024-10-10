using BethanysPieShop.Mobile.ViewModels;
using BethanysPieShop.Mobile.Views.Base;

namespace BethanysPieShop.Mobile.Views;

public partial class ShoppingCartPage
{
    public ShoppingCartPage(ShoppingCartViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}