using Depom.Domain.Entities;

namespace Depom.Infrastructure.User.Persistence.Interfaces;

public interface IUserRepository
{
    Task<List<AppUser>> GetAllAsync();
    Task<AppUser?> GetByIdAsync(int id);
    Task<AppUser?> GetByUsernameAsync(string username);
    Task AddAsync(AppUser entity);
    Task UpdateAsync(AppUser entity);
    Task DeleteAsync(int id);
    Task<bool> UsernameExistsAsync(string username, int? excludeId = null);
}