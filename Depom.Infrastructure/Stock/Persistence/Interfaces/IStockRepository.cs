using Depom.Domain.Entities;

namespace Depom.Infrastructure.Stock.Persistence.Interfaces;

public interface IStockRepository
{
    Task<List<StockItem>> GetByBranchAsync(int branchId);
    Task<List<StockItem>> GetByProductAsync(int productId);
    Task<StockItem?> GetAsync(int productId, int branchId);
    Task AddAsync(StockItem entity);
    Task UpdateAsync(StockItem entity);
    Task AddMovementAsync(StockMovement movement);
    Task<List<StockMovement>> GetMovementsAsync(int branchId, int? productId = null);
}
