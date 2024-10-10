using System.Collections.ObjectModel;
using AutoMapper;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class PiesOverviewViewModel : ViewModelBase
{
    private readonly ICategoryService _categoryService;
    private readonly IPieService _pieService;
    private readonly IMapper _mapper;
    private readonly INavigationService _navigationService;

    [ObservableProperty] 
    private bool _isRefreshing;

    [ObservableProperty]
    private ObservableCollection<CategoryViewModel> _categories = new();

    [RelayCommand]
    private async Task Refresh()
    {
        await GetData(true);
        IsRefreshing = false;
    }

    public PiesOverviewViewModel(
        INavigationService navigationService, 
        ICategoryService categoryService, 
        IPieService pieService,
        IMapper mapper) 
        : base(navigationService)
    {
        _navigationService = navigationService;
        _categoryService = categoryService;
        _pieService = pieService;
        _mapper = mapper;
    }
    
    public override async Task InitializeAsync()
    {
        await Loading(() => GetData());
    }

    private async Task GetData(bool retrieveFromServer = false)
    {
        Categories.Clear();

        List<CategoryModel> categoryModels = await _categoryService.GetAllCategories(retrieveFromServer);
        foreach (var category in categoryModels)
        {
            Categories.Add(MapCategoryModelToViewModel(category));
        }

        List<PieModel> pieModels = await _pieService.GetAllPies(retrieveFromServer);
        foreach (var category in Categories)
        {
            var pies = pieModels.Where(p => p.CategoryId == category.Id).ToList();
            foreach (var pie in _mapper.Map<List<PieViewModel>>(pies))
            {
                category.Pies.Add(pie);
            }
        }
    }

    private CategoryViewModel MapCategoryModelToViewModel(CategoryModel category) 
        => new(_navigationService, category.Id, category.Label);
}