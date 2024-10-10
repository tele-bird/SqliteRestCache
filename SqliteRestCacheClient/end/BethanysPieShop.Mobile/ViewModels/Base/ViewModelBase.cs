using BethanysPieShop.Mobile.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace BethanysPieShop.Mobile.ViewModels.Base;

public abstract partial class ViewModelBase : ObservableValidator, IViewModelBase
{
    public INavigationService NavigationService { get; }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoaded))]
    private bool _isLoading;

    public bool IsLoaded => !IsLoading;

    [RelayCommand]
    private async Task GoBack()
        => await NavigationService.GoBack();

    protected ViewModelBase(INavigationService navigationService)
    {
        NavigationService = navigationService;
        InitializeAsyncCommand = new AsyncRelayCommand(
            async () =>
            {
                IsLoading = true;
                await Loading(InitializeAsync);
                IsLoading = false;
            });
    }

    public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected async Task Loading(Func<Task> unitOfWork)
    {
        await unitOfWork();
    }

    protected async Task ExecuteWithErrorHandlingAsync(Func<Task> operation, string? exceptionMessage = null)
    {
        try
        {
            await operation();
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex, exceptionMessage);
        }
    }

    private async Task HandleErrorAsync(Exception ex, string? exceptionMessage = null)
    {
        // Log the error (you can implement logging here)
        Debug.WriteLine(ex);

        // Show a user-friendly message
        if (string.IsNullOrEmpty(exceptionMessage))
        {
            await Shell.Current.DisplayAlert("Error", "Something went wrong. Please try again.", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", $"{exceptionMessage}.", "OK");
        }
    }
}