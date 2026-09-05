
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductManager.Application.MessagingQueue.Messages;
using ProductManager.Domain.Data;
using ProductManager.Domain.Models;
using ProductManager.Dtos;
using System.Data;

namespace ProductManager.Application.Services.ProductService;

public class ProductService:IProductService
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
        if(p == null) return null;
        return _mapper.Map<ProductDto>(p);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto)
    {
        Product p = null;
        var product = _mapper.Map<Product>(dto);
        product.Id = Guid.NewGuid();
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> UpdateAsync(ProductDto dto)
    {
        var existing = await _db.Products.FirstOrDefaultAsync(x => x.Id == dto.Id);
        if(existing == null) return false;
        _mapper.Map(dto, existing);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if(existing == null) return false;
        _db.Products.Remove(existing);
        await _db.SaveChangesAsync();
        return true;
    }


    public async Task<bool> ReserveStockAsync(ICollection<OrderItem> orderItems)
    {
        var orderItemsAsTable = new DataTable();

        orderItemsAsTable.Columns.Add(
            DbConstants.ProductIdColumn,
            typeof(Guid));

        orderItemsAsTable.Columns.Add(
            DbConstants.ProductQuantityColumn,
            typeof(int));

        foreach(var item in orderItems)
        {
            orderItemsAsTable.Rows.Add(
                item.ProductId,
                item.ProductQuantity);
        }

        var parameter = new SqlParameter(
            DbConstants.OrderItemsParameter,
            orderItemsAsTable)
        {
            SqlDbType = SqlDbType.Structured,
            TypeName = DbConstants.OrderItemTableType
        };

        var result = (await _db.Database
            .SqlQueryRaw<bool>(
                DbConstants.ReserveStockCommand,
                parameter).ToListAsync<bool>()).First<bool>();


        return result;
    }


    public async Task<bool> ReleaseStockAsync(ICollection<OrderItem> orderItems)
    {
        throw new NotImplementedException();
    }
}

