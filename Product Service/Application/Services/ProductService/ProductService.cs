
using AutoMapper;
using global::Product_Service.Data;
using global::Product_Service.Dtos;
using global::Product_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Product_Service.Application.Services.ProductService;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ProductService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _db.Products.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var p = await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (p == null) return null;
        return _mapper.Map<ProductDto>(p);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto)
    {
        var product = _mapper.Map<Product>(dto);
        if (product.Id == Guid.Empty)
            product.Id = Guid.NewGuid();
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> UpdateAsync(ProductDto dto)
    {
        var existing = await _db.Products.FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (existing == null) return false;
        _mapper.Map(dto, existing);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (existing == null) return false;
        _db.Products.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }
}

