# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GrpcServices/InventoryService/InventoryService.API/InventoryService.API.csproj", "GrpcServices/InventoryService/InventoryService.API/"]
COPY ["GrpcServices/InventoryService/InventoryService.Application/InventoryService.Application.csproj", "GrpcServices/InventoryService/InventoryService.Application/"]
COPY ["GrpcServices/InventoryService/InventoryService.Domain/InventoryService.Domain.csproj", "GrpcServices/InventoryService/InventoryService.Domain/"]
COPY ["GrpcServices/InventoryService/InventoryService.Infrastructure/InventoryService.Infrastructure.csproj", "GrpcServices/InventoryService/InventoryService.Infrastructure/"]
RUN dotnet restore "./GrpcServices/InventoryService/InventoryService.API/InventoryService.API.csproj"
COPY . .
WORKDIR "/src/GrpcServices/InventoryService/InventoryService.API"
RUN dotnet build "./InventoryService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InventoryService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryService.API.dll"]