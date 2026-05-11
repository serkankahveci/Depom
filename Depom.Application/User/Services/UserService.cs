using Depom.Domain.Entities;
using Depom.Domain.Enums;
using Depom.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Depom.Application.User.Services;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AppUser?> ValidateAsync(string username, string password)
    {
        var user = await _context.AppUsers
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Username == username && x.IsActive);

        if (user == null) return null;

        // Basit hash kontrolu - production'da BCrypt kullanilmali
        var hash = Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(password)));

        return user.PasswordHash == hash ? user : null;
    }

    public async Task SeedAdminAsync()
    {
        if (await _context.AppUsers.AnyAsync()) return;

        var hash = Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes("admin123")));

        _context.AppUsers.Add(new AppUser
        {
            FullName     = "Sistem Yoneticisi",
            Username     = "admin",
            PasswordHash = hash,
            Role         = AppRole.SystemAdmin,
            IsActive     = true
        });

        await _context.SaveChangesAsync();
    }

    public async Task<AppUser?> GetByIdAsync(int id)
        => await _context.AppUsers
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Id == id);
}
