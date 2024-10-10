using AutoMapper;
using BethanysPieShop.API.DTO;
using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderLine, OrderLineDto>().ReverseMap();
        CreateMap<OrderCreateDto, Order>();
        CreateMap<OrderLineCreateDto, OrderLine>();
        CreateMap<ShippingInformation, ShippingInformationDto>().ReverseMap();
    }
}