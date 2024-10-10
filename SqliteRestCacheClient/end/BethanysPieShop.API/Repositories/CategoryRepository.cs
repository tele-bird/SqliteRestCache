using BethanysPieShop.API.DBContexts;
using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly BethanysPieShopDbContext _context;

    public CategoryRepository(BethanysPieShopDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<List<Category>> GetCategories()
    {
        return await _context.Categories
            .ToListAsync();
    }
}