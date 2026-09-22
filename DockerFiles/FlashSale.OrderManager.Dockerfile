# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for better layer caching
COPY ["FlashSale.OrderManager.API.sln", "./"]

COPY ["FlashSale.OrderManager.API/FlashSale.OrderManager.API.csproj", "FlashSale.OrderManager.API/"]
COPY ["FlashSale.OrderManager.Application/FlashSale.OrderManager.Application.csproj", "FlashSale.OrderManager.Application/"]
COPY ["FlashSale.OrderManager.Domain/FlashSale.OrderManager.Domain.csproj", "FlashSale.OrderManager.Domain/"]
COPY ["FlashSale.OrderManager.Infrastructure/FlashSale.OrderManager.Infrastructure.csproj", "FlashSale.OrderManager.Infrastructure/"]

RUN dotnet restore "FlashSale.OrderManager.API.sln"

COPY . .

RUN dotnet publish \
    "FlashSale.OrderManager.API/FlashSale.OrderManager.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "FlashSale.OrderManager.API.dll"]