using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Services.Interfaces;

public interface IPieService
{
    Task<List<Pie>> GetAllPies();
    
    Task<List<Pie>> GetPiesOfTheWeek();
    
    Task<Pie?> GetPieById(int id);
}