using AutoMapper;
using Depom.Application.Transfer.DTOs;
using Depom.Domain.Entities;
using Depom.Domain.Enums;
using Depom.Infrastructure.Stock.Persistence.Interfaces;
using Depom.Infrastructure.Transfer.Persistence.Interfaces;

namespace Depom.Application.Transfer.Services;

public class TransferService
{
    private readonly ITransferRepository _transferRepo;
    private readonly IStockRepository _stockRepo;
    private readonly IMapper _mapper;

    public TransferService(
        ITransferRepository transferRepo,
        IStockRepository stockRepo,
        IMapper mapper)
    {
        _transferRepo = transferRepo;
        _stockRepo = stockRepo;
        _mapper = mapper;
    }

    public async Task<List<TransferListDto>> GetAllAsync()
    {
        var items = await _transferRepo.GetAllAsync();
        return _mapper.Map<List<TransferListDto>>(items);
    }

    public async Task<List<TransferListDto>> GetByBranchAsync(int branchId)
    {
        var items = await _transferRepo.GetByBranchAsync(branchId);
        return _mapper.Map<List<TransferListDto>>(items);
    }

    public async Task<TransferDetailDto?> GetByIdAsync(int id)
    {
        var item = await _transferRepo.GetByIdWithLogsAsync(id);
        return item == null ? null : _mapper.Map<TransferDetailDto>(item);
    }

    public async Task<int> CreateAsync(TransferCreateDto dto, int userId)
    {
        var entity = _mapper.Map<Domain.Entities.Transfer>(dto);
        entity.CreatedByUserId = userId;
        entity.Status = TransferStatus.Draft;
        await _transferRepo.AddAsync(entity);
        return entity.Id;
    }

    public async Task SubmitAsync(TransferActionDto dto, int userId)
        => await ChangeStatusAsync(dto.TransferId, TransferStatus.Pending, userId, dto.Note);

    public async Task ApproveAsync(TransferActionDto dto, int userId)
        => await ChangeStatusAsync(dto.TransferId, TransferStatus.Approved, userId, dto.Note);

    public async Task ShipAsync(TransferActionDto dto, int userId)
    {
        var transfer = await _transferRepo.GetByIdWithLogsAsync(dto.TransferId);
        if (transfer == null) return;

        var sourceStock = await _stockRepo.GetAsync(transfer.ProductId, transfer.SourceBranchId);
        if (sourceStock != null)
        {
            sourceStock.Quantity -= transfer.Quantity;
            sourceStock.ReservedQuantity -= transfer.Quantity;
            await _stockRepo.UpdateAsync(sourceStock);
        }

        await _stockRepo.AddMovementAsync(new StockMovement
        {
            ProductId = transfer.ProductId,
            BranchId = transfer.SourceBranchId,
            Type = StockMovementType.Transfer,
            Quantity = transfer.Quantity,
            TransferId = transfer.Id,
            CreatedByUserId = userId,
            Note = $"Transfer #{transfer.Id} - sevk edildi"
        });

        await ChangeStatusAsync(dto.TransferId, TransferStatus.InTransit, userId, dto.Note);
    }

    public async Task ReceiveAsync(TransferActionDto dto, int userId)
    {
        var transfer = await _transferRepo.GetByIdWithLogsAsync(dto.TransferId);
        if (transfer == null) return;

        var targetStock = await _stockRepo.GetAsync(transfer.ProductId, transfer.TargetBranchId);
        if (targetStock == null)
        {
            targetStock = new StockItem
            {
                ProductId = transfer.ProductId,
                BranchId = transfer.TargetBranchId,
                Quantity = transfer.Quantity
            };
            await _stockRepo.AddAsync(targetStock);
        }
        else
        {
            targetStock.Quantity += transfer.Quantity;
            await _stockRepo.UpdateAsync(targetStock);
        }

        await _stockRepo.AddMovementAsync(new StockMovement
        {
            ProductId = transfer.ProductId,
            BranchId = transfer.TargetBranchId,
            Type = StockMovementType.Transfer,
            Quantity = transfer.Quantity,
            TransferId = transfer.Id,
            CreatedByUserId = userId,
            Note = $"Transfer #{transfer.Id} - teslim alindi"
        });

        await ChangeStatusAsync(dto.TransferId, TransferStatus.Received, userId, dto.Note);
    }

    public async Task CancelAsync(TransferActionDto dto, int userId)
    {
        var transfer = await _transferRepo.GetByIdWithLogsAsync(dto.TransferId);
        if (transfer == null) return;

        if (transfer.Status == TransferStatus.Pending)
        {
            var sourceStock = await _stockRepo.GetAsync(transfer.ProductId, transfer.SourceBranchId);
            if (sourceStock != null)
            {
                sourceStock.ReservedQuantity -= transfer.Quantity;
                await _stockRepo.UpdateAsync(sourceStock);
            }
        }

        await ChangeStatusAsync(dto.TransferId, TransferStatus.Cancelled, userId, dto.Note);
    }

    private async Task ChangeStatusAsync(int transferId, TransferStatus newStatus, int userId, string? note)
    {
        var transfer = await _transferRepo.GetByIdWithLogsAsync(transferId);
        if (transfer == null) return;

        var log = new TransferLog
        {
            TransferId = transferId,
            OldStatus = transfer.Status,
            NewStatus = newStatus,
            ActingUserId = userId,
            Note = note
        };

        transfer.Status = newStatus;
        await _transferRepo.UpdateAsync(transfer);
        await _transferRepo.AddLogAsync(log);
    }
}
