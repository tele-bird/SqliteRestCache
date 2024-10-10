using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;
using BethanysPieShop.Mobile.Services.Interfaces;

namespace BethanysPieShop.Mobile.Services;

public class PieService : IPieService
{
    private readonly IPieRepository _pieRepository;

    public PieService(IPieRepository pieRepository)
        => _pieRepository = pieRepository;

    public Task<PieDetailModel?> GetPieById(int id)
        => _pieRepository.GetPieById(id);

    public Task<List<PieModel>> GetPiesOfTheWeek()
        => _pieRepository.GetPiesOfTheWeek();

    public Task<List<PieModel>> GetAllPies(bool retrieveFromServer = false)
        => _pieRepository.GetAllPies(retrieveFromServer);
}