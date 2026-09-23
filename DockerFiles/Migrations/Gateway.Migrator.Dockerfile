FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /src

COPY . .

RUN dotnet tool install --global dotnet-ef --version 8.*

ENV PATH="/root/.dotnet/tools:${PATH}"

ENTRYPOINT ["dotnet", "ef", "database", "update", \
    "--project", "GateWay/Gateway.Infrastructure/Gateway.Infrastructure.csproj", \
    "--startup-project", "GateWay/GateWay/GateWay.API.csproj"]