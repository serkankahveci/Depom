using Depom.Infrastructure.Branch.Persistence.Interfaces;
using Depom.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Depom.Infrastructure.Branch.Persistence.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Domain.Entities.Branch>> GetAllAsync()
        => await _context.Branches
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();

    public async Task<Domain.Entities.Branch?> GetByIdAsync(int id)
        => await _context.Branches.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Domain.Entities.Branch entity)
    {
        await _context.Branches.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Domain.Entities.Branch entity)
    {
        _context.Branches.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Branches.FindAsync(id);
        if (entity != null)
        {
            entity.IsActive = false;
            await _context.SaveChangesAsync();
        }
    }
}
