using BethanysPieShop.API.Entities;

namespace BethanysPieShop.API.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetCategories();
}