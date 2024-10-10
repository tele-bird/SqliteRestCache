using AutoMapper;
using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.ViewModels;

namespace BethanysPieShop.Mobile.Mappings;

public class PieProfile : Profile
{
    public PieProfile()
    {
        CreateMap<PieModel, PieViewModel>();
    }
}