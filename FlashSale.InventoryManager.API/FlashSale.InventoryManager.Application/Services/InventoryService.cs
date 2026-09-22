
using AutoMapper;
using FlashSale.InventoryManager.Application.Dtos;
using FlashSale.InventoryManager.Application.Inerfaces;
using FlashSale.InventoryManager.Application.Interfaces;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCancelled;
using FlashSale.InventoryManager.Application.Messaging.Messages.OrderCreated;
using FlashSale.InventoryManager.Domain.Enums;
using FlashSale.InventoryManager.Domain.Models;
using FlashSale.InventoryManager.Infrastructure.Redis;
using System.Data;


namespace FlashSale.InventoryManager.Application.Services.InventoryService;

public class InventoryService:IInventoryService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRedisDistributedLock redisDistributedLock;

    public InventoryService(IUnitOfWork unitOfWork, IMapper mapper, IRedisDistributedLock redisDistributedLock)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        this.redisDistributedLock = redisDistributedLock;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {

        var products = await _unitOfWork.Inventory.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var p = await _unitOfWork.Inventory.GetByIdAsync(id);
        if(p is null) return null;
        return _mapper.Map<ProductDto>(p);
    }

    public async Task<ProductDto> CreateAsync(ProductDto dto)
    {
        Product p = new Product();
        var product = _mapper.Map<Product>(dto);


        await _unitOfWork.Inventory.CreateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<Product> UpdateAsync(ProductDto dto)
    {
        Product product = _mapper.Map<Product>(dto);
        await _unitOfWork.Inventory.UpdateAsync(product);
        var result = await _unitOfWork.SaveChangesAsync();
        if(result == 0)
        {
            //log ;
            return null;
        }

        return product;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await _unitOfWork.Inventory.DeleteAsync(id);
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }


    public async Task<bool> ReserveStockAsync(Guid orderId, ICollection<OrderCreatedProduct> orderProductList)
    {

        var productIds = orderProductList.Select(p => p.ProductId.ToString());

        var locksDictionary = await redisDistributedLock.AcquireMultipleAsync
            (DistributedLockTypes.ProductInventoryLock.ToString(),
            productIds, RedisOptions.DefaultLockExpiry, RedisOptions.DefaultMaxWait);

        var result = await _unitOfWork.Inventory.ReserveStockAsync(orderId, orderProductList);

        await redisDistributedLock.ReleaseMultipleAsync(
            DistributedLockTypes.ProductInventoryLock.ToString(), locksDictionary);

        return result;
    }


    public async Task<bool> ReleaseStockAsync(Guid orderId, ICollection<OrderCancelledProduct> orderProductList)
    {
        var productIds = orderProductList.Select(p => p.ProductId.ToString());

        var locksDictionary = await redisDistributedLock.AcquireMultipleAsync
            (DistributedLockTypes.ProductInventoryLock.ToString(),
            productIds, RedisOptions.DefaultLockExpiry, RedisOptions.DefaultMaxWait);

        var result = await _unitOfWork.Inventory.ReleaseStockAsync(orderId, orderProductList);

        await redisDistributedLock.ReleaseMultipleAsync(
            DistributedLockTypes.ProductInventoryLock.ToString(), locksDictionary);

        return result;
    }
}

