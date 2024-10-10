using AutoMapper;
using BethanysPieShop.API.DTO;
using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Mappings;

public class PieProfile : Profile
{
    public PieProfile()
    {
        CreateMap<Pie, PieDto>().ReverseMap();
        CreateMap<Pie, PieDetailDto>();
    }
}