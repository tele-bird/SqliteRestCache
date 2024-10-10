using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Repositories.Interfaces;

public interface IPieRepository
{
    Task<PieDetailModel?> GetPieById(int id);
    
    Task<List<PieModel>> GetPiesOfTheWeek();
    
    Task<List<PieModel>> GetAllPies(bool retrieveFromServer = false);
}