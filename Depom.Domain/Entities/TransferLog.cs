using Depom.Domain.Enums;

namespace Depom.Domain.Entities;

public class TransferLog
{
    public int Id { get; set; }
    public TransferStatus OldStatus { get; set; }
    public TransferStatus NewStatus { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int TransferId { get; set; }
    public int ActingUserId { get; set; }

    // Navigation
    public Transfer Transfer { get; set; } = null!;
    public AppUser ActingUser { get; set; } = null!;
}
