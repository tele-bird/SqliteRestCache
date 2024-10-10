using System.Collections.ObjectModel;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class CategoryViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    
    public int Id { get; }

    public string Label { get; }

    public ObservableCollection<PieViewModel> Pies { get; } = new();

    [ObservableProperty] 
    private PieViewModel? _selectedPie;

    [RelayCommand]
    private async Task NavigateToSelectedPieDetail()
    {
        if (SelectedPie is not null)
        {
            await _navigationService.GoToSelectedPieDetail(SelectedPie.Id);
            SelectedPie = null;
        }
    }
    
    public CategoryViewModel(
        INavigationService navigationService,
        int id,
        string label)
        : base(navigationService)
    {
        _navigationService = navigationService;
        Id = id;
        Label = label;
    }
}