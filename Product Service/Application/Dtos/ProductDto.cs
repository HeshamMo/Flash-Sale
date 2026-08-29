using System;

namespace Product_Service.Dtos;

public record ProductDto(Guid Id, string Name, string? Description, decimal Price, int Quantity);
