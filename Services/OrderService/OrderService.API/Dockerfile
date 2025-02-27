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
COPY ["Services/OrderService/OrderService.API/OrderService.API.csproj", "Services/OrderService/OrderService.API/"]
COPY ["Services/OrderService/OrderService.Application/OrderService.Application.csproj", "Services/OrderService/OrderService.Application/"]
COPY ["Services/OrderService/OrderService.Domain/OrderService.Domain.csproj", "Services/OrderService/OrderService.Domain/"]
COPY ["Services/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "Services/OrderService/OrderService.Infrastructure/"]
RUN dotnet restore "./Services/OrderService/OrderService.API/OrderService.API.csproj"
COPY . .
WORKDIR "/src/Services/OrderService/OrderService.API"
RUN dotnet build "./OrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]