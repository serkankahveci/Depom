using AutoMapper;
using Depom.Application.Stock.DTOs;
using Depom.Domain.Entities;
using Depom.Domain.Enums;
using Depom.Infrastructure.Stock.Persistence.Interfaces;

namespace Depom.Application.Stock.Services;

public class StockService
{
    private readonly IStockRepository _repo;
    private readonly IMapper _mapper;

    public StockService(IStockRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<StockItemDto>> GetByBranchAsync(int branchId)
    {
        var items = await _repo.GetByBranchAsync(branchId);
        return _mapper.Map<List<StockItemDto>>(items);
    }

    public async Task<List<StockItemDto>> GetByProductAsync(int productId)
    {
        var items = await _repo.GetByProductAsync(productId);
        return _mapper.Map<List<StockItemDto>>(items);
    }

    public async Task AddMovementAsync(StockMovementCreateDto dto, int userId)
    {
        var stock = await _repo.GetAsync(dto.ProductId, dto.BranchId);
        if (stock == null)
        {
            stock = new StockItem
            {
                ProductId = dto.ProductId,
                BranchId = dto.BranchId,
                Quantity = 0
            };
            await _repo.AddAsync(stock);
        }

        if (dto.Type == StockMovementType.In)
            stock.Quantity += dto.Quantity;
        else if (dto.Type == StockMovementType.Out)
            stock.Quantity -= dto.Quantity;

        await _repo.UpdateAsync(stock);

        var movement = new StockMovement
        {
            ProductId = dto.ProductId,
            BranchId = dto.BranchId,
            Type = dto.Type,
            Quantity = dto.Quantity,
            Note = dto.Note,
            CreatedByUserId = userId
        };
        await _repo.AddMovementAsync(movement);
    }

    public async Task AdjustAsync(StockAdjustDto dto, int userId)
    {
        var stock = await _repo.GetAsync(dto.ProductId, dto.BranchId);
        int oldQty = stock?.Quantity ?? 0;
        int diff = dto.NewQuantity - oldQty;

        if (stock == null)
        {
            stock = new StockItem
            {
                ProductId = dto.ProductId,
                BranchId = dto.BranchId,
                Quantity = dto.NewQuantity
            };
            await _repo.AddAsync(stock);
        }
        else
        {
            stock.Quantity = dto.NewQuantity;
            await _repo.UpdateAsync(stock);
        }

        var movement = new StockMovement
        {
            ProductId = dto.ProductId,
            BranchId = dto.BranchId,
            Type = StockMovementType.Adjust,
            Quantity = Math.Abs(diff),
            Note = dto.Note ?? $"Duzeltme: {oldQty} -> {dto.NewQuantity}",
            CreatedByUserId = userId
        };
        await _repo.AddMovementAsync(movement);
    }

    public async Task<List<StockMovementListDto>> GetMovementsAsync(int branchId, int? productId = null)
    {
        var items = await _repo.GetMovementsAsync(branchId, productId);
        return _mapper.Map<List<StockMovementListDto>>(items);
    }
}
