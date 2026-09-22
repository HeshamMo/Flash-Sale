# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for better layer caching
COPY ["GateWay.sln", "./"]
COPY ["GateWay/GateWay.API.csproj", "GateWay/"]
COPY ["Gateway.Application/Gateway.Application.csproj", "Gateway.Application/"]
COPY ["Gateway.Domain/Gateway.Domain.csproj", "Gateway.Domain/"]
COPY ["Gateway.Infrastructure/Gateway.Infrastructure.csproj", "Gateway.Infrastructure/"]


RUN dotnet restore "GateWay.sln"

# Copy the source code
COPY . .

# Publish the Gateway API
RUN dotnet publish "GateWay/GateWay.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "GateWay.API.dll"]
