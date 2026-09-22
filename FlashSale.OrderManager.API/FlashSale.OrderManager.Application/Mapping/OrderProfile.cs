using AutoMapper;
using FlashSale.OrderManager.Application.DTOs.Orders;
using FlashSale.OrderManager.Application.Messaging.Messages;
using FlashSale.OrderManager.Domain.Models;

namespace OrderManager.Application.Mapping;

public class OrderProfile:Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderRequest, Order>()
                    .ForMember(
                        dest => dest.Id,
                        opt => opt.Ignore())
                    .ForMember(
                        dest => dest.CustomerId,
                        opt => opt.Ignore())
                    .ForMember(
                        dest => dest.Status,
                        opt => opt.Ignore())
                    .ForMember(
                        dest => dest.CreatedAtUtc,
                        opt => opt.Ignore());


        CreateMap<CreateOrderItemRequest, OrderProducts>();


        CreateMap<Order, OrderResponse>();
        CreateMap<OrderProducts, OrderProductResponse>();


        CreateMap<Order, OrderCreatedMessage>()
                .ForMember(
                    dest => dest.OrderId,
                    opt => opt.MapFrom(src => src.Id));


        CreateMap<OrderProducts, OrderCreatedProduct>()
            .ForMember(
                dest => dest.ProductId,
                opt => opt.MapFrom(src => src.ProductId))
            .ForMember(
                dest => dest.ProductQuantity,
                opt => opt.MapFrom(src => src.Quantity));
    }
}