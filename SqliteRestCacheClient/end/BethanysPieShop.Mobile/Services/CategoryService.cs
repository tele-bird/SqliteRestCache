using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;
using BethanysPieShop.Mobile.Services.Interfaces;

namespace BethanysPieShop.Mobile.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public Task<List<CategoryModel>> GetAllCategories(bool retrieveFromServer = false)
        => _categoryRepository.GetAllCategories(retrieveFromServer);
}