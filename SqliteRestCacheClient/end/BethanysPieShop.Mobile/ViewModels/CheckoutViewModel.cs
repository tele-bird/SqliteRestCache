using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class CheckoutViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IOrderService _orderService;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _firstName = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _lastName = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _addressLine1 = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _zipCode = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _city = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _state = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _country = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _phoneNumber = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string _email = default!;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string? _cardName;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string? _cardNumber;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string? _cardExpiration;

    [ObservableProperty, NotifyDataErrorInfo, Required]
    private string? _cvvCode;

    private bool CanSubmitOrder() => !HasErrors;

    [RelayCommand(CanExecute = nameof(CanSubmitOrder))]
    private async Task Submit()
    {
        await ExecuteWithErrorHandlingAsync(async () =>
            {
                var orderCreateModel = new OrderCreateModel
                {
                    ShippingInformation = new ShippingInformationModel
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        AddressLine1 = AddressLine1,
                        ZipCode = ZipCode,
                        City = City,
                        State = State,
                        Country = Country,
                        PhoneNumber = PhoneNumber,
                        Email = Email
                    },
                    OrderLines = new List<OrderLineCreateModel>()
                };

                var shoppingCart = await _shoppingCartService.GetShoppingCart();
                foreach (ShoppingCartItemModel item in shoppingCart.Items)
                {
                    orderCreateModel.OrderLines.Add(new OrderLineCreateModel
                    {
                        PieId = item.PieId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Pie.Price
                    });
                }

                long orderId = await _orderService.CreateOrder(orderCreateModel);
                await Shell.Current.DisplayAlert("Order successful", $"Order with Id {orderId} has been created", "OK");

                await _shoppingCartService.ClearShoppingCart();
                await _navigationService.GoBack();
                await _navigationService.GoTo("//Home");
            }, "Something went wrong when creating your order");
    }

    public CheckoutViewModel(
        INavigationService navigationService,
        IShoppingCartService shoppingCartService,
        IOrderService orderService)
        : base(navigationService)
    {
        _navigationService = navigationService;
        _shoppingCartService = shoppingCartService;
        _orderService = orderService;

        ErrorsChanged += CheckoutViewModel_ErrorsChanged;

        ValidateAllProperties();
    }

    public ObservableCollection<ValidationResult> Errors { get; } = new();

    private void CheckoutViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        Errors.Clear();
        GetErrors().ToList().ForEach(Errors.Add);
        SubmitCommand.NotifyCanExecuteChanged();
    }
}