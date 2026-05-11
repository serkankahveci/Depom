namespace Depom.Domain.Entities;

public class Branch
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    public ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    public ICollection<Transfer> OutgoingTransfers { get; set; } = new List<Transfer>();
    public ICollection<Transfer> IncomingTransfers { get; set; } = new List<Transfer>();
}
