using AutoMapper;
using BethanysPieShop.API.DTO;
using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Mappings;

public class ShoppingCartProfile: Profile
{
    public ShoppingCartProfile()
    {
        CreateMap<ShoppingCart, ShoppingCartDto>().ReverseMap();

        CreateMap<ShoppingCartItem, ShoppingCartItemDto>().ReverseMap();
    }
}