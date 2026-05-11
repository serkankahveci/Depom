using Depom.Domain.Entities;

namespace Depom.Infrastructure.Product.Persistence.Interfaces;

public interface IProductRepository
{
    Task<List<Domain.Entities.Product>> GetAllAsync();
    Task<List<Domain.Entities.Product>> GetByCategoryAsync(int categoryId);
    Task<Domain.Entities.Product?> GetByIdAsync(int id);
    Task<Domain.Entities.Product?> GetBySkuAsync(string sku);
    Task AddAsync(Domain.Entities.Product entity);
    Task UpdateAsync(Domain.Entities.Product entity);
    Task DeleteAsync(int id);
}
