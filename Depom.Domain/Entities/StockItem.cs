namespace Depom.Domain.Entities;

public class StockItem
{
    public int Id { get; set; }
    public int Quantity { get; set; } = 0;
    public int ReservedQuantity { get; set; } = 0;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int ProductId { get; set; }
    public int BranchId { get; set; }

    // Navigation
    public Product Product { get; set; } = null!;
    public Branch Branch { get; set; } = null!;

    // Hesaplanan alan
    public int AvailableQuantity => Quantity - ReservedQuantity;
}
