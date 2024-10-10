using AutoMapper;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.ViewModels;
using TrackingServiceReference;

namespace BethanysPieShop.Mobile.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderLineViewModel, OrderLineModel>().ReverseMap();

        CreateMap<TrackingInformationViewModel, TrackingInformation>().ReverseMap();
    }
}