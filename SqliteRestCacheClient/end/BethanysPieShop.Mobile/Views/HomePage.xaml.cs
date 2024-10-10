using BethanysPieShop.Mobile.ViewModels;

namespace BethanysPieShop.Mobile.Views;

public partial class HomePage
{
    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}