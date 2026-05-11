using System.ComponentModel.DataAnnotations;

namespace Depom.Application.Category.DTOs;

public class CategoryCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CategoryListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
