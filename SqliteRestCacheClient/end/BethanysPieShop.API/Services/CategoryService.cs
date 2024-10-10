using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using BethanysPieShop.API.Services.Interfaces;

namespace BethanysPieShop.API.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<List<Category>> GetCategories()
        => _categoryRepository.GetCategories();
}