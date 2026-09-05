using AutoMapper;
using ProductManager.Domain.Models;
using ProductManager.Dtos;


namespace ProductManager.Application.Mapper;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }

}
