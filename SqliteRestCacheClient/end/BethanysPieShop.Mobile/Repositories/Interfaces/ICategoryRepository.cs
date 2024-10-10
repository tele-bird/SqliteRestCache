using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<CategoryModel>> GetAllCategories(bool retrieveFromServer = false);
}