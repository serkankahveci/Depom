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
            .Select(x => new CategoryListDto { Id = x.Id, Name = x.Name, IsActive = x.IsActive })
            .ToListAsync();

    public async Task CreateAsync(CategoryCreateDto dto)
    {
        _context.Categories.Add(new Domain.Entities.Category
        {
            Name        = dto.Name,
            Description = dto.Description
        });
        await _context.SaveChangesAsync();
    }
}
