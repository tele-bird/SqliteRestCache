using BethanysPieShop.Mobile.ViewModels;
using BethanysPieShop.Mobile.Views.Base;

namespace BethanysPieShop.Mobile.Views;

public partial class LoginPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}