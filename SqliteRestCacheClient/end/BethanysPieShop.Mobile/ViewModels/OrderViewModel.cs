using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class OrderViewModel : ViewModelBase
{
    [ObservableProperty] 
    private long _id;

    [ObservableProperty] 
    private DateTime _orderDate;

    [ObservableProperty] 
    private decimal _orderTotal;

    public OrderViewModel(
        INavigationService navigationService,
        long id,
        DateTime orderDate,
        decimal orderTotal) 
        : base(navigationService)
    {
        Id = id;
        OrderDate = orderDate;
        OrderTotal = orderTotal;
    }
}