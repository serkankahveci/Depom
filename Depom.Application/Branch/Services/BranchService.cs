using AutoMapper;
using Depom.Application.Branch.DTOs;
using Depom.Infrastructure.Branch.Persistence.Interfaces;

namespace Depom.Application.Branch.Services;

public class BranchService
{
    private readonly IBranchRepository _repo;
    private readonly IMapper _mapper;

    public BranchService(IBranchRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<BranchListDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<BranchListDto>>(items);
    }

    public async Task<BranchDetailDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : _mapper.Map<BranchDetailDto>(item);
    }

    public async Task CreateAsync(BranchCreateDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.Branch>(dto);
        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(BranchUpdateDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.Branch>(dto);
        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
        => await _repo.DeleteAsync(id);
}
