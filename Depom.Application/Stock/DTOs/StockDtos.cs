using Depom.Domain.Enums;

namespace Depom.Application.Stock.DTOs;

public class StockItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public int LowStockThreshold { get; set; }
    public bool IsLowStock => Quantity <= LowStockThreshold;
}

public class StockMovementCreateDto
{
    public int ProductId { get; set; }
    public int BranchId { get; set; }
    public StockMovementType Type { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
}

public class StockMovementListDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public StockMovementType Type { get; set; }
    public int Quantity { get; set; }
    public string? Note { get; set; }
    public string CreatedByUserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class StockAdjustDto
{
    public int ProductId { get; set; }
    public int BranchId { get; set; }
    public int NewQuantity { get; set; }
    public string? Note { get; set; }
}
