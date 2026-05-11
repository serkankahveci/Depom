using Depom.Domain.Entities;
using Depom.Domain.Enums;

namespace Depom.Infrastructure.Transfer.Persistence.Interfaces;

public interface ITransferRepository
{
    Task<List<Domain.Entities.Transfer>> GetAllAsync();
    Task<List<Domain.Entities.Transfer>> GetByBranchAsync(int branchId);
    Task<List<Domain.Entities.Transfer>> GetByStatusAsync(TransferStatus status);
    Task<Domain.Entities.Transfer?> GetByIdWithLogsAsync(int id);
    Task AddAsync(Domain.Entities.Transfer entity);
    Task UpdateAsync(Domain.Entities.Transfer entity);
    Task AddLogAsync(TransferLog log);
}
