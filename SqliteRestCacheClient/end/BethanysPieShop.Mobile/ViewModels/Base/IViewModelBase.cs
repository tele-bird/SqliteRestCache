using BethanysPieShop.Mobile.Services.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels.Base;

public interface IViewModelBase : IQueryAttributable
{
    public INavigationService NavigationService { get; }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public bool IsLoading { get; }

    Task InitializeAsync();
}