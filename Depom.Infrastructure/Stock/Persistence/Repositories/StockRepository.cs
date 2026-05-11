using Depom.Domain.Entities;
using Depom.Infrastructure.Persistence.Context;
using Depom.Infrastructure.Stock.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Depom.Infrastructure.Stock.Persistence.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockItem>> GetByBranchAsync(int branchId)
        => await _context.StockItems
            .Include(x => x.Product)
            .Where(x => x.BranchId == branchId)
            .ToListAsync();

    public async Task<List<StockItem>> GetByProductAsync(int productId)
        => await _context.StockItems
            .Include(x => x.Branch)
            .Where(x => x.ProductId == productId)
            .ToListAsync();

    public async Task<StockItem?> GetAsync(int productId, int branchId)
        => await _context.StockItems
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.BranchId == branchId);

    public async Task AddAsync(StockItem entity)
    {
        await _context.StockItems.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StockItem entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.StockItems.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddMovementAsync(StockMovement movement)
    {
        await _context.StockMovements.AddAsync(movement);
        await _context.SaveChangesAsync();
    }

    public async Task<List<StockMovement>> GetMovementsAsync(int branchId, int? productId = null)
    {
        var query = _context.StockMovements
            .Include(x => x.Product)
            .Include(x => x.CreatedByUser)
            .Where(x => x.BranchId == branchId);

        if (productId.HasValue)
            query = query.Where(x => x.ProductId == productId.Value);

        return await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
    }
}
