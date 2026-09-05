using ProductManager.Application.MessagingQueue.Messages;
using ProductManager.Dtos;


namespace ProductManager.Application.Services.ProductService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<ProductDto> CreateAsync(ProductDto dto);
    Task<bool> UpdateAsync(ProductDto dto);
    Task<bool> DeleteAsync(Guid id);

    Task<bool> ReserveStockAsync(ICollection<OrderItem> orderItems);
    Task<bool> ReleaseStockAsync(ICollection<OrderItem> orderItems);



}
