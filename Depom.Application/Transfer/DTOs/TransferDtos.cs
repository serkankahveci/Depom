using Depom.Domain.Enums;

namespace Depom.Application.Transfer.DTOs;

public class TransferCreateDto
{
    public int ProductId { get; set; }
    public int SourceBranchId { get; set; }
    public int TargetBranchId { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
}

public class TransferListDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SourceBranchName { get; set; } = string.Empty;
    public string TargetBranchName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public TransferStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TransferDetailDto : TransferListDto
{
    public int ProductId { get; set; }
    public int SourceBranchId { get; set; }
    public int TargetBranchId { get; set; }
    public string? Note { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public List<TransferLogDto> Logs { get; set; } = new();
}

public class TransferLogDto
{
    public TransferStatus OldStatus { get; set; }
    public TransferStatus NewStatus { get; set; }
    public string ActingUserName { get; set; } = string.Empty;
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TransferActionDto
{
    public int TransferId { get; set; }
    public string? Note { get; set; }
}
