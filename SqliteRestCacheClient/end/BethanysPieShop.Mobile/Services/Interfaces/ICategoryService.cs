using BethanysPieShop.Mobile.Models;

namespace BethanysPieShop.Mobile.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryModel>> GetAllCategories(bool retrieveFromServer = false);
}