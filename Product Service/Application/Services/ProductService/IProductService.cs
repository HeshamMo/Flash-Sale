using System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Product_Service.Dtos;

namespace Product_Service.Application.Services.ProductService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<ProductDto> CreateAsync(ProductDto dto);
    Task<bool> UpdateAsync(ProductDto dto);
    Task<bool> DeleteAsync(Guid id);
}
