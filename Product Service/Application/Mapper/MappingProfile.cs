using AutoMapper;
using Product_Service.Dtos;
using Product_Service.Models;

namespace Product_Service.Application.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
