using Depom.Infrastructure.Persistence.Context;
using Depom.Infrastructure.Product.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Depom.Infrastructure.Product.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Domain.Entities.Product>> GetAllAsync()
        => await _context.Products
            .Include(x => x.Category)
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();

    public async Task<List<Domain.Entities.Product>> GetByCategoryAsync(int categoryId)
        => await _context.Products
            .Where(x => x.CategoryId == categoryId && x.IsActive)
            .ToListAsync();

    public async Task<Domain.Entities.Product?> GetByIdAsync(int id)
        => await _context.Products
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Domain.Entities.Product?> GetBySkuAsync(string sku)
        => await _context.Products.FirstOrDefaultAsync(x => x.SKU == sku);

    public async Task AddAsync(Domain.Entities.Product entity)
    {
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Domain.Entities.Product entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Products.FindAsync(id);
        if (entity != null)
        {
            entity.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
