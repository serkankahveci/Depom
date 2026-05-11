using Depom.Domain.Enums;

namespace Depom.Domain.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public AppRole Role { get; set; } = AppRole.Viewer;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // Foreign Keys
    public int? BranchId { get; set; }

    // Navigation
    public Branch? Branch { get; set; }
}
