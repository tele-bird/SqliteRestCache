using System.Collections.ObjectModel;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class ShoppingCartViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IAuthService _authService;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasItemsInCart))]
    private ObservableCollection<ShoppingCartItemViewModel> _items = [];

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(HasNoItemsInCart))]
    private bool _hasItemsInCart;
    
    public bool HasNoItemsInCart => !HasItemsInCart;
    
    [RelayCommand]
    private async Task Delete(ShoppingCartItemViewModel item)
    {
        await _shoppingCartService.Delete(item.Id);
        Items.Remove(item);
        HasItemsInCart = Items.Count > 0;
    }
    
    [RelayCommand]
    private async Task GoToCheckout()
    {

        bool isLoggedIn = await _authService.IsLoggedIn();

        if (isLoggedIn)
        {
            await _navigationService.GoTo("Checkout");
        }
        else
        {
            await _navigationService.GoToLogin("Checkout");
        }
    }
    
    public ShoppingCartViewModel(
        INavigationService navigationService, 
        IShoppingCartService shoppingCartService, 
        IAuthService authService) 
        : base(navigationService)
    {
        _navigationService = navigationService;
        _shoppingCartService = shoppingCartService;
        _authService = authService;
    }
    
    public override async Task InitializeAsync()
    {
        await Loading(GetData);
    }

    private async Task GetData()
    {
        var shoppingCart = await _shoppingCartService.GetShoppingCart();
        Items.Clear();
        
        foreach (var itemvm in shoppingCart.Items.Select(item => new ShoppingCartItemViewModel(
                     _navigationService,
                     _shoppingCartService,
                     item.Id,
                     item.Pie.Name,
                     item.Pie.Price,
                     item.Pie.ImageThumbnailUrl,
                     item.Quantity,
                     item.PieId)))
        {
            Items.Add(itemvm);
        }

        HasItemsInCart = Items.Count > 0;
    }
}