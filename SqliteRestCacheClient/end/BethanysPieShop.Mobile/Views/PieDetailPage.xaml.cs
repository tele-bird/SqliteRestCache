using BethanysPieShop.Mobile.ViewModels;

namespace BethanysPieShop.Mobile.Views;

public partial class PieDetailPage
{
    public PieDetailPage(PieDetailViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}