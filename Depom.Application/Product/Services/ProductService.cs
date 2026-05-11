using AutoMapper;
using Depom.Application.Product.DTOs;
using Depom.Infrastructure.Product.Persistence.Interfaces;

namespace Depom.Application.Product.Services;

public class ProductService
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<ProductListDto>> GetAllAsync()
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<List<ProductListDto>>(items);
    }

    public async Task<ProductDetailDto?> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id);
        return item == null ? null : _mapper.Map<ProductDetailDto>(item);
    }

    public async Task CreateAsync(ProductCreateDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.Product>(dto);
        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(ProductUpdateDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.Product>(dto);
        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(int id)
        => await _repo.DeleteAsync(id);
}
