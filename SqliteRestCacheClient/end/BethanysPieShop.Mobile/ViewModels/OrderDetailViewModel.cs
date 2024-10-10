using System.Collections.ObjectModel;
using System.Text;
using AutoMapper;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Services.Interfaces;
using BethanysPieShop.Mobile.ViewModels.Base;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TrackingServiceReference;

namespace BethanysPieShop.Mobile.ViewModels;

[QueryProperty(nameof(Id), "OrderId")]
public partial class OrderDetailViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly IOrderService _orderService;
    private readonly IDeliveryTrackingService _deliveryTrackingService;

    [ObservableProperty] 
    private int _id;

    [ObservableProperty] 
    private DateTime _orderDate;

    [ObservableProperty] 
    private decimal _orderTotal;

    [ObservableProperty] 
    private ObservableCollection<OrderLineViewModel> _orderLines = new();

    [ObservableProperty] 
    private ShippingInformationViewModel? _shippingInformation;

    [ObservableProperty] 
    private long _trackAndTraceCode;

    [ObservableProperty] 
    private string? _deliveryAddress;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HideTrackingInformation))]
    private bool _showTrackingInformation;

    [ObservableProperty] 
    private ObservableCollection<TrackingInformationViewModel> _trackingInformation = new();

    public bool HideTrackingInformation => !ShowTrackingInformation;

    [RelayCommand]
    private async Task ToggleTrackingInformation()
    {
        if (!ShowTrackingInformation && TrackingInformation.Count == 0 && TrackAndTraceCode != 0)
        {
            TrackingInformation[] trackingInformation = await _deliveryTrackingService.GetTrackingInformation(TrackAndTraceCode);
            TrackingInformation = _mapper.Map<ObservableCollection<TrackingInformationViewModel>>(trackingInformation);
        }
        ShowTrackingInformation = !ShowTrackingInformation;
    }

    [RelayCommand]
    private async Task DownloadOrderDetail()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"Order {Id}");
        builder.AppendLine($"OrderItems:");
        foreach (var orderLine in OrderLines)
        {
            builder.AppendLine($"{orderLine.Quantity} x {orderLine.Pie.Name}: ${orderLine.TotalPrice}");
        }

        var content = builder.ToString();

        await SaveFile($"Order_{Id}.txt", content);
    }

    public OrderDetailViewModel(
        INavigationService navigationService,
        IOrderService orderService,
        IMapper mapper, IDeliveryTrackingService deliveryTrackingService)
        : base(navigationService)
    {
        _orderService = orderService;
        _mapper = mapper;
        _deliveryTrackingService = deliveryTrackingService;
    }

    public override async Task InitializeAsync()
    {
        await Loading(GetData);
        ShowTrackingInformation = false;
    }

    private async Task GetData()
    {
        if (Id != 0)
        {
            var orderDetail = await _orderService.GetOrderById(Id);
            if (orderDetail == null)
            {
                GoBackCommand.Execute(null);
            }
            else
            {
                MapOrderDetailModelToViewModel(orderDetail);
            }
        }
    }

    private void MapOrderDetailModelToViewModel(OrderDetailModel orderDetail)
    {
        Id = orderDetail.Id;
        OrderDate = orderDetail.OrderDate;
        OrderTotal = orderDetail.OrderTotal;
        ShippingInformation = _mapper.Map<ShippingInformationViewModel>(orderDetail.ShippingInformation);
        OrderLines = MapOrderLinesModelToViewModel(orderDetail.OrderLines);
        TrackAndTraceCode = orderDetail.TrackAndTraceCode;
        DeliveryAddress =
            $"{ShippingInformation.FirstName} {ShippingInformation.LastName}, {ShippingInformation.AddressLine1}, {ShippingInformation.ZipCode} {ShippingInformation.City}";
    }

    private ObservableCollection<OrderLineViewModel> MapOrderLinesModelToViewModel(List<OrderLineModel> orderDetailOrderLines) 
        => _mapper.Map<ObservableCollection<OrderLineViewModel>>(orderDetailOrderLines);

    private async Task SaveFile(string filename, string content)
    {
        using var stream = new MemoryStream(Encoding.Default.GetBytes(content));
        var fileSaverResult = await FileSaver.Default.SaveAsync(filename, stream);
        string displayContent;

        if (fileSaverResult.IsSuccessful)
        {
            displayContent = $"The file was saved successfully to location: {fileSaverResult.FilePath}";
        }
        else
        {
            displayContent = $"The file was not saved successfully with error: {fileSaverResult.Exception.Message}";
        }

        await Shell.Current.DisplayAlert("", displayContent, "OK");
    }
}