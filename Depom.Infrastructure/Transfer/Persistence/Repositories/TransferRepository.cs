using Depom.Domain.Entities;
using Depom.Domain.Enums;
using Depom.Infrastructure.Persistence.Context;
using Depom.Infrastructure.Transfer.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Depom.Infrastructure.Transfer.Persistence.Repositories;

public class TransferRepository : ITransferRepository
{
    private readonly AppDbContext _context;

    public TransferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Domain.Entities.Transfer>> GetAllAsync()
        => await _context.Transfers
            .Include(x => x.Product)
            .Include(x => x.SourceBranch)
            .Include(x => x.TargetBranch)
            .Include(x => x.CreatedByUser)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public async Task<List<Domain.Entities.Transfer>> GetByBranchAsync(int branchId)
        => await _context.Transfers
            .Include(x => x.Product)
            .Include(x => x.SourceBranch)
            .Include(x => x.TargetBranch)
            .Where(x => x.SourceBranchId == branchId || x.TargetBranchId == branchId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public async Task<List<Domain.Entities.Transfer>> GetByStatusAsync(TransferStatus status)
        => await _context.Transfers
            .Include(x => x.Product)
            .Include(x => x.SourceBranch)
            .Include(x => x.TargetBranch)
            .Where(x => x.Status == status)
            .ToListAsync();

    public async Task<Domain.Entities.Transfer?> GetByIdWithLogsAsync(int id)
        => await _context.Transfers
            .Include(x => x.Product)
            .Include(x => x.SourceBranch)
            .Include(x => x.TargetBranch)
            .Include(x => x.CreatedByUser)
            .Include(x => x.Logs)
                .ThenInclude(x => x.ActingUser)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Domain.Entities.Transfer entity)
    {
        await _context.Transfers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Domain.Entities.Transfer entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Transfers.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task AddLogAsync(TransferLog log)
    {
        await _context.TransferLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
