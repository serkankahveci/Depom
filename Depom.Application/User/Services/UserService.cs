using AutoMapper;
using Depom.Application.User.DTOs;
using Depom.Domain.Entities;
using Depom.Domain.Enums;
using Depom.Infrastructure.Persistence.Context;
using Depom.Infrastructure.User.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Depom.Application.User.Services;

public class UserService
{
    private readonly IUserRepository _repo;
    private readonly AppDbContext    _context; // Seed + ValidateAsync iÃ§in
    private readonly IMapper         _mapper;

    public UserService(IUserRepository repo, AppDbContext context, IMapper mapper)
    {
        _repo    = repo;
        _context = context;
        _mapper  = mapper;
    }

    // â”€â”€ Auth â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    public async Task<AppUser?> ValidateAsync(string username, string password)
    {
        var user = await _context.AppUsers
            .Include(x => x.Branch)
            .FirstOrDefaultAsync(x => x.Username == username && x.IsActive);

        if (user == null) return null;

        return user.PasswordHash == HashPassword(password) ? user : null;
    }

    public async Task SeedAdminAsync()
    {
        if (await _context.AppUsers.AnyAsync()) return;

        _context.AppUsers.Add(new AppUser
        {
            FullName     = "Sistem YÃ¶neticisi",
            Username     = "admin",
            PasswordHash = HashPassword("admin123"),
            Role         = AppRole.SystemAdmin,
            IsActive     = true
        });

        await _context.SaveChangesAsync();
    }

    // â”€â”€ CRUD â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    public async Task<List<UserListDto>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return _mapper.Map<List<UserListDto>>(users);
    }

    public async Task<UserDetailDto?> GetByIdAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user == null ? null : _mapper.Map<UserDetailDto>(user);
    }

    public async Task<AppUser?> GetEntityByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    /// <returns>null = baÅŸarÄ±lÄ±, string = hata mesajÄ±</returns>
    public async Task<string?> CreateAsync(UserCreateDto dto)
    {
        if (await _repo.UsernameExistsAsync(dto.Username))
            return $"'{dto.Username}' kullanÄ±cÄ± adÄ± zaten kullanÄ±mda.";

        var entity = _mapper.Map<AppUser>(dto);
        entity.PasswordHash = HashPassword(dto.Password);

        await _repo.AddAsync(entity);
        return null;
    }

    /// <returns>null = baÅŸarÄ±lÄ±, string = hata mesajÄ±</returns>
    public async Task<string?> UpdateAsync(UserUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity == null) return "KullanÄ±cÄ± bulunamadÄ±.";

        if (await _repo.UsernameExistsAsync(dto.Username, dto.Id))
            return $"'{dto.Username}' kullanÄ±cÄ± adÄ± zaten kullanÄ±mda.";

        _mapper.Map(dto, entity);

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            entity.PasswordHash = HashPassword(dto.NewPassword);

        await _repo.UpdateAsync(entity);
        return null;
    }

    public async Task DeleteAsync(int id)
        => await _repo.DeleteAsync(id);

    public async Task<bool> UsernameExistsAsync(string username, int? excludeId = null)
        => await _repo.UsernameExistsAsync(username, excludeId);

    // â”€â”€ Helpers â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    private static string HashPassword(string password)
        => Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(password)));
}