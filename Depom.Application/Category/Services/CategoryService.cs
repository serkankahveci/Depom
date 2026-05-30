using Depom.Application.Category.DTOs;
using Depom.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Depom.Application.Category.Services;

public class CategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryListDto>> GetAllAsync()
        => await _context.Categories
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new CategoryListDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .ToListAsync();

    public async Task<CategoryListDto?> GetByIdAsync(int id)
        => await _context.Categories
            .Where(x => x.Id == id)
            .Select(x => new CategoryListDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();

    public async Task CreateAsync(CategoryCreateDto dto)
    {
        _context.Categories.Add(new Domain.Entities.Category
        {
            Name = dto.Name,
            Description = dto.Description
        });
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, CategoryCreateDto dto)
    {
        var entity = await _context.Categories.FindAsync(id);
        if (entity == null) return;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Categories.FindAsync(id);
        if (entity == null) return;
        entity.IsActive = false;
        await _context.SaveChangesAsync();
    }
}