# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for better layer caching
COPY ["FlashSale.InventoryManager.API.sln", "./"]

COPY ["FlashSale.InventoryManager.API/FlashSale.InventoryManager.API.csproj", "FlashSale.InventoryManager.API/"]
COPY ["FlashSale.InventoryManager.Application/FlashSale.InventoryManager.Application.csproj", "FlashSale.InventoryManager.Application/"]
COPY ["FlashSale.InventoryManager.Domain/FlashSale.InventoryManager.Domain.csproj", "FlashSale.InventoryManager.Domain/"]
COPY ["FlashSale.InventoryManager.Infrastructure/FlashSale.InventoryManager.Infrastructure.csproj", "FlashSale.InventoryManager.Infrastructure/"]

RUN dotnet restore "FlashSale.InventoryManager.API.sln"

# Copy the source code
COPY . .

# Publish the Inventory API
RUN dotnet publish "FlashSale.InventoryManager.API/FlashSale.InventoryManager.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "FlashSale.InventoryManager.API.dll"]
