using Depom.Domain.Enums;

namespace Depom.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public decimal Price { get; set; }
    public int LowStockThreshold { get; set; } = 5;
    public ProductStatus Status { get; set; } = ProductStatus.Active;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int CategoryId { get; set; }

    // Navigation
    public Category Category { get; set; } = null!;
    public ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
