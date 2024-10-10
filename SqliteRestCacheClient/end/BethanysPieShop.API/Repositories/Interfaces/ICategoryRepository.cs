using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategories();
}