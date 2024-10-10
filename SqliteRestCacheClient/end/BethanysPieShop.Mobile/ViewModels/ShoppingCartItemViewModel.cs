using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class ShoppingCartItemViewModel : ViewModelBase
{
    private readonly IShoppingCartService _shoppingCartService;

    [ObservableProperty] 
    private int _id;

    [ObservableProperty] 
    private string _name;

    [ObservableProperty] 
    private decimal _price;

    [ObservableProperty] 
    private string? _imageThumbnailUrl;

    [ObservableProperty] 
    private string? _shortDescription;

    [ObservableProperty] 
    private int _pieId;

    private int _quantity;

    private bool updateQuantityOnServer = true;

    public int Quantity
    {
        get => _quantity;
        set => new Task(async () =>
        {
            await SetQuantityAsync(value);
        }).Start();
    }

    private async Task SetQuantityAsync(int value)
    {
        if (_quantity != value)
        {
            _quantity = value;
            OnPropertyChanged(nameof(Quantity));
            OnPropertyChanged(nameof(TotalPrice));

            if (updateQuantityOnServer)
            {
                await UpdateItem();

            }
        }

        updateQuantityOnServer = true;
    }

    private async Task UpdateItem()
    {
        var item = new ShoppingCartItemModel
        {
            Id = Id,
            Quantity = Quantity,
            PieId = PieId
        };

        await _shoppingCartService.Update(item);
    }

    public decimal TotalPrice => Quantity * Price;

    public ShoppingCartItemViewModel(
        INavigationService navigationService,
        IShoppingCartService shoppingCartService,
        int id, 
        string name, 
        decimal price, 
        string? imageThumbnailUrl,
        int quantity,
        int pieId)
        : base(navigationService)
    {
        _shoppingCartService = shoppingCartService;

        Id = id;
        Name = name;
        Price = price;
        ImageThumbnailUrl = imageThumbnailUrl;
        Quantity = quantity;
        PieId = pieId;

        updateQuantityOnServer = false;
    }
}