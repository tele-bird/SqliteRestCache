using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

[QueryProperty(nameof(Id), "PieId")]
public partial class PieDetailViewModel : ViewModelBase
{
    private readonly IPieService _pieService;
    private readonly INavigationService _navigationService;
    private readonly IShoppingCartService _shoppingCartService;

    [ObservableProperty] 
    private int _id;

    [ObservableProperty] 
    private string _name = default!;

    [ObservableProperty] 
    private decimal _price;

    [ObservableProperty] 
    private string? _imageUrl;

    [ObservableProperty] 
    private string? _imageThumbnailUrl;

    [ObservableProperty] 
    private string? _shortDescription;

    [ObservableProperty] 
    private string? _longDescription;

    [ObservableProperty] 
    private int? _categoryId;

    [ObservableProperty] 
    private CategoryViewModel? _category;

    [RelayCommand]
    private async Task AddToCart()
    {
        var shoppingCartItem = new ShoppingCartItemModel
        {
            Quantity = 1,
            PieId = Id,
            Pie = new PieModel()
            {
                Id = Id,
                Name = Name,
                Price = Price,
                ImageThumbnailUrl = ImageThumbnailUrl,
            }
        };

        await _shoppingCartService.AddItem(shoppingCartItem);
        
        await Shell.Current.DisplayAlert("Item added to cart", "", "OK");
    }
    
    public PieDetailViewModel(
        INavigationService navigationService, 
        IPieService pieService, 
        IShoppingCartService shoppingCartService) 
        : base(navigationService)
    {
        _navigationService = navigationService;
        _pieService = pieService;
        _shoppingCartService = shoppingCartService;
    }
    
    public override async Task InitializeAsync()
    {
        await Loading(GetData);
    }

    private async Task GetData()
    {
        if (Id != 0)
        {
            var pieDetail = await _pieService.GetPieById(Id);
            if (pieDetail is not null)
            {
                MapPieDetailModelToViewModel(pieDetail);
            }
            else
            {
                GoBackCommand.Execute(null);
            }
        }
    }

    private void MapPieDetailModelToViewModel(PieDetailModel pie)
    {
        Id = pie.Id;
        Name = pie.Name;
        Price = pie.Price;
        ImageUrl = pie.ImageUrl;
        ImageThumbnailUrl = pie.ImageThumbnailUrl;
        ShortDescription = pie.ShortDescription;
        LongDescription = pie.LongDescription;
        CategoryId = pie.CategoryId;
        if (pie.Category is not null)
        {
            Category = new CategoryViewModel(_navigationService, pie.Category.Id, pie.Category.Label);
        }
    }
}