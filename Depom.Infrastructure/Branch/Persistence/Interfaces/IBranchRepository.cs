using Depom.Domain.Entities;

namespace Depom.Infrastructure.Branch.Persistence.Interfaces;

public interface IBranchRepository
{
    Task<List<Domain.Entities.Branch>> GetAllAsync();
    Task<Domain.Entities.Branch?> GetByIdAsync(int id);
    Task AddAsync(Domain.Entities.Branch entity);
    Task UpdateAsync(Domain.Entities.Branch entity);
    Task DeleteAsync(int id);
}
