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
COPY ["Arlequim.Stock.Api/Arlequim.Stock.Api.csproj", "Arlequim.Stock.Api/"]
COPY ["Arlequim.Stock.Application/Arlequim.Stock.Application.csproj", "Arlequim.Stock.Application/"]
COPY ["Arlequim.Stock.Domain/Arlequim.Stock.Domain.csproj", "Arlequim.Stock.Domain/"]
COPY ["Arlequim.Stock.Infrastructure/Arlequim.Stock.Infrastructure.csproj", "Arlequim.Stock.Infrastructure/"]
RUN dotnet restore "./Arlequim.Stock.Api/Arlequim.Stock.Api.csproj"
COPY . .
WORKDIR "/src/Arlequim.Stock.Api"
RUN dotnet build "./Arlequim.Stock.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Arlequim.Stock.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Arlequim.Stock.Api.dll"]