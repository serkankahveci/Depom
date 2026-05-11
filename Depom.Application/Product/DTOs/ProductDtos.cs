using Depom.Domain.Enums;

namespace Depom.Application.Product.DTOs;

public class ProductCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public int LowStockThreshold { get; set; } = 5;
    public int CategoryId { get; set; }
}

public class ProductUpdateDto : ProductCreateDto
{
    public int Id { get; set; }
    public ProductStatus Status { get; set; }
}

public class ProductListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public ProductStatus Status { get; set; }
    public bool IsActive { get; set; }
}

public class ProductDetailDto : ProductListDto
{
    public string? Description { get; set; }
    public int LowStockThreshold { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
