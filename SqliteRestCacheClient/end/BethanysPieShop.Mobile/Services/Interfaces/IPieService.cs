using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Services.Interfaces;

public interface IPieService
{
    Task<PieDetailModel?> GetPieById(int id);
    
    Task<List<PieModel>> GetPiesOfTheWeek();
    
    Task<List<PieModel>> GetAllPies(bool retrieveFromServer = false);
}