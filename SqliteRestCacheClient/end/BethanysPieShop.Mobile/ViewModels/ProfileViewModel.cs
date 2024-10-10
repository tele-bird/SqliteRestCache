using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class ProfileViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IAuthService _authService;
    
    [ObservableProperty]
    private string? _email;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoggedOut))]
    private bool _isLoggedIn;

    public bool IsLoggedOut => !IsLoggedIn;

    [RelayCommand]
    private async Task GoToOrderHistory()
    {
        await _navigationService.GoTo("OrderHistory");
    }
    
    [RelayCommand]
    private async Task Logout()
    {
        bool logout = await Shell.Current.DisplayAlert("Log out", "Are you sure you want to log out?", "Yes", "Cancel");
        if (logout)
        {
            try
            {
                await _authService.Logout();
                IsLoggedIn = false;
                Email = null;
            }
            catch (Exception)
            {
                await Shell.Current.DisplayAlert("Log out failed", "", "OK");
            }
        } 
    }
    
    [RelayCommand]
    private async Task Login()
    {
        await _navigationService.GoToLogin("//MyAccount");
        await GetData();
    }
    
    public ProfileViewModel(
        INavigationService navigationService, IAuthService authService) 
        : base(navigationService)
    {
        _navigationService = navigationService;
        _authService = authService;
    }
    
    public override async Task InitializeAsync()
    {
        await Loading(GetData);
    }

    private async Task GetData()
    {
        IsLoggedIn = await _authService.IsLoggedIn();
        Email = IsLoggedIn ? await _authService.GetUserEmail() : null;
    }
}