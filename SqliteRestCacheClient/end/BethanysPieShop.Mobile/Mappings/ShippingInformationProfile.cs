using AutoMapper;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.ViewModels;

namespace BethanysPieShop.Mobile.Mappings;

public class ShippingInformationProfile : Profile
{
    public ShippingInformationProfile()
    {
        CreateMap<ShippingInformationModel, ShippingInformationViewModel>().ReverseMap();
    }
}