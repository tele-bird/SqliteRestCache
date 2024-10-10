using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

[QueryProperty(nameof(ReturnUrl), "ReturnUrl")]
public partial class LoginViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IAuthService _authService;
    
    [ObservableProperty] 
    private string? _returnUrl;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _email = default!;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _password = default!;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task Login()
    {
        try
        {
            await _authService.Login(Email, Password);
            if (ReturnUrl is null)
            {
                await _navigationService.GoTo("//Home");
            }
            else
            {
                await _navigationService.GoTo(ReturnUrl);
            }
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Login failed", "", "OK");
        }
    }

    [RelayCommand]
    private async Task Register()
    {
        await _navigationService.GoToRegister(ReturnUrl);
    }

    private bool CanLogin()
        => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
    
    public LoginViewModel(
        INavigationService navigationService, 
        IAuthService authService) 
        : base(navigationService)
    {
        _navigationService = navigationService;
        _authService = authService;
    }
}