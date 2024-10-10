using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

[QueryProperty(nameof(ReturnUrl), "ReturnUrl")]
public partial class RegisterViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    
    [ObservableProperty] 
    private string? _returnUrl;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _email = default!;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _password = default!;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RegisterCommand))]
    private string _confirmPassword = default!;

    [RelayCommand(CanExecute = nameof(CanRegister))]
    private async Task Register()
    {
        try
        {
            await _authService.Register(Email, Password);
            await Shell.Current.DisplayAlert("Registration successful", "", "OK");
            await _navigationService.GoToLogin(ReturnUrl);
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert("Registration failed", "", "OK");
        }
    }

    [RelayCommand]
    private async Task Login()
    {
        await _navigationService.GoToLogin(ReturnUrl);
    }

    private bool CanRegister()
        => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) && Password == ConfirmPassword;
    
    public RegisterViewModel(
        INavigationService navigationService,
        IAuthService authService) 
        : base(navigationService)
    {
        _navigationService = navigationService;
        _authService = authService;
    }
}