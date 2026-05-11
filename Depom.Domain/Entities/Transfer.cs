using Depom.Domain.Enums;

namespace Depom.Domain.Entities;

public class Transfer
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public TransferStatus Status { get; set; } = TransferStatus.Draft;
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public int SourceBranchId { get; set; }
    public int TargetBranchId { get; set; }
    public int CreatedByUserId { get; set; }

    // Navigation
    public Product Product { get; set; } = null!;
    public Branch SourceBranch { get; set; } = null!;
    public Branch TargetBranch { get; set; } = null!;
    public AppUser CreatedByUser { get; set; } = null!;
    public ICollection<TransferLog> Logs { get; set; } = new List<TransferLog>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}
