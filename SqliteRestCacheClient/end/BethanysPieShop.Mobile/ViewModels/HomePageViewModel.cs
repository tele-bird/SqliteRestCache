using System.Collections.ObjectModel;
using AutoMapper;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    private readonly IPieService _pieService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<PieViewModel> _piesOfTheWeek = new();

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
    
    public HomePageViewModel(
        INavigationService navigationService, 
        IPieService pieService,
        IMapper mapper) 
        : base(navigationService)
    {
        _navigationService = navigationService;
        _pieService = pieService;
        _mapper = mapper;
    }
    
    public override async Task InitializeAsync()
    {
        await Loading(GetData);
    }

    private async Task GetData()
    {
        List<PieModel> pieModels = await _pieService.GetPiesOfTheWeek();
        PiesOfTheWeek.Clear();

        PiesOfTheWeek = _mapper.Map<ObservableCollection<PieViewModel>>(pieModels);
    }
}