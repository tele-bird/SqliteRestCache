using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using BethanysPieShop.API.Services.Interfaces;

namespace BethanysPieShop.API.Services;

public class PieService : IPieService
{
    private readonly IPieRepository _pieRepository;

    public PieService(
        IPieRepository pieRepository)
    {
        _pieRepository = pieRepository;
    }

    public Task<List<Pie>> GetAllPies()
        => _pieRepository.GetAllPies();

    public Task<List<Pie>> GetPiesOfTheWeek()
        => _pieRepository.GetPiesOfTheWeek();

    public Task<Pie?> GetPieById(int id)
        => _pieRepository.GetPieById(id);
}