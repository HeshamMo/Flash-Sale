using AutoMapper;
using FlashSale.OrderManager.Application.DTOs.Orders;
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


        CreateMap<CreateOrderItemRequest, OrderItem>();
        CreateMap<Order, OrderResponse>();
    }
}