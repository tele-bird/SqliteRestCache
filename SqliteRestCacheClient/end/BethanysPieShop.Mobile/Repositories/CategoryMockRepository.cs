using BethanysPieShop.Mobile.Models;
using BethanysPieShop.Mobile.Repositories.Interfaces;

namespace BethanysPieShop.Mobile.Repositories;

public class CategoryMockRepository : ICategoryRepository
{
    public Task<List<CategoryModel>> GetAllCategories(bool retrieveFromServer)
    {
        return Task.FromResult(_categoryList);
    }
    
    private List<CategoryModel> _categoryList =
    [
        new(id: 1, label: "Fruit Pies"),
        new(id: 2, label: "Cheese cakes"),
        new(id: 3, label: "Seasonal Pies")
    ];
}