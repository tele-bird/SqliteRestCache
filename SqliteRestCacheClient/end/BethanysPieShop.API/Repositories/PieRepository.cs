using BethanysPieShop.API.DBContexts;
using BethanysPieShop.API.Entities;
using BethanysPieShop.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.API.Repositories;

public class PieRepository : IPieRepository
{
    private readonly BethanysPieShopDbContext _context;

    public PieRepository(BethanysPieShopDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<List<Pie>> GetAllPies() 
        => await _context.Pies
            .ToListAsync();

    public async Task<List<Pie>> GetPiesOfTheWeek() 
        => await _context.Pies
            .Where(p => p.IsPieOfTheWeek)
            .ToListAsync();

    public async Task<Pie?> GetPieById(int id) 
        => await _context.Pies
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
}