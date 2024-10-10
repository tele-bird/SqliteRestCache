using AutoMapper;
using BethanysPieShop.API.DTO;
using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}