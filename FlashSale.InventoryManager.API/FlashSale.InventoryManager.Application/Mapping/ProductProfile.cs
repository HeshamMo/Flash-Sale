using AutoMapper;
using FlashSale.InventoryManager.Application.Dtos;
using FlashSale.InventoryManager.Domain.Models;



namespace FlashSale.InventoryManager.Application.Mapping;

public class ProductProfile:Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }

}
