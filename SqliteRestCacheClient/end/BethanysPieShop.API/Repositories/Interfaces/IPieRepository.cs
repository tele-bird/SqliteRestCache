using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Repositories.Interfaces;

public interface IPieRepository
{
    Task<List<Pie>> GetAllPies();
    
    Task<List<Pie>> GetPiesOfTheWeek();
    
    Task<Pie?> GetPieById(int id);
}