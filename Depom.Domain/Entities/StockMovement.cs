using Depom.Domain.Enums;

namespace Depom.Domain.Entities;

public class StockMovement
{
    public int Id { get; set; }
    public StockMovementType Type { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int ProductId { get; set; }
    public int BranchId { get; set; }
    public int CreatedByUserId { get; set; }
    public int? TransferId { get; set; }

    // Navigation
    public Product Product { get; set; } = null!;
    public Branch Branch { get; set; } = null!;
    public AppUser CreatedByUser { get; set; } = null!;
    public Transfer? Transfer { get; set; }
}
