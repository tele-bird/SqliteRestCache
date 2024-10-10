using BethanysPieShop.Mobile.ViewModels.Base;

namespace BethanysPieShop.Mobile.Views.Base;

public abstract class ContentPageBase : ContentPage
{
    protected ContentPageBase()
    {
        NavigationPage.SetBackButtonTitle(this, string.Empty);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is not IViewModelBase ivmb)
        {
            return;
        }

        await ivmb.InitializeAsyncCommand.ExecuteAsync(null);
    }
}