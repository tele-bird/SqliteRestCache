using BethanysPieShop.Mobile.ViewModels;

namespace BethanysPieShop.Mobile.Views;

public partial class PiesOverviewPage
{
    public PiesOverviewPage(PiesOverviewViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}