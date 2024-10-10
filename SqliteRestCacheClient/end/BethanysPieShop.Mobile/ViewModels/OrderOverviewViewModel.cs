using System.Collections.ObjectModel;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BethanysPieShop.Mobile.ViewModels;

public partial class OrderOverviewViewModel : ViewModelBase
{
    private readonly IOrderService _orderService;
    private readonly INavigationService _navigationService;
    
    [ObservableProperty]
    private ObservableCollection<OrderViewModel> _orders = new();
    
    [ObservableProperty] 
    private OrderViewModel? _selectedOrder;
    
    [RelayCommand]
    private async Task NavigateToSelectedOrderDetail()
    {
        if (SelectedOrder is not null)
        {
            await _navigationService.GoToSelectedOrderDetail(SelectedOrder.Id);
            SelectedOrder = null;
        }
    }
    
    public OrderOverviewViewModel(
        INavigationService navigationService, 
        IOrderService orderService) 
        : base(navigationService)
    {
        _navigationService = navigationService;
        _orderService = orderService;
    }
    
    public override async Task InitializeAsync()
    {
        await Loading(GetData);
    }

    private async Task GetData()
    {
        Orders.Clear();

        List<OrderModel> orderModels = await _orderService.GetOrderHistory();
        foreach (OrderModel order in orderModels)
        {
            Orders.Add(MapOrderModelToViewModel(order));
        }
    }

    private OrderViewModel MapOrderModelToViewModel(OrderModel order)
    {
        return new OrderViewModel(_navigationService, order.Id, order.OrderDate, order.OrderTotal);
    }
}