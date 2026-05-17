namespace Depom.Application.Branch.DTOs;

public class BranchCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
}

public class BranchUpdateDto : BranchCreateDto
{
    public int Id { get; set; }
}

public class BranchListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
}

public class BranchDetailDto : BranchListDto
{
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}