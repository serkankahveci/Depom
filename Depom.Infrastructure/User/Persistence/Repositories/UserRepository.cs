using Depom.Domain.Entities;
using Depom.Infrastructure.Persistence.Context;
using Depom.Infrastructure.User.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Depom.Infrastructure.User.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AppUser>> GetAllAsync()
        => await _context.AppUsers
            .Include(x => x.Branch)
            .OrderBy(x => x.FullName)
            .ToListAsync();

    public async Task<AppUser?> GetByIdAsync(int id)
        => await _context.AppUsers
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<AppUser?> GetByUsernameAsync(string username)
        => await _context.AppUsers
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Username == username);

    public async Task AddAsync(AppUser entity)
    {
        await _context.AppUsers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AppUser entity)
    {
        _context.AppUsers.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.AppUsers.FindAsync(id);
        if (entity != null)
        {
            _context.AppUsers.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> UsernameExistsAsync(string username, int? excludeId = null)
        => await _context.AppUsers
            .AnyAsync(x => x.Username == username && (excludeId == null || x.Id != excludeId));
}