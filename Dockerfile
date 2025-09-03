# Use the official .NET 9 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InventoryApi.csproj", "."]
RUN dotnet restore "InventoryApi.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "InventoryApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "InventoryApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create Data directory for SQLite database
RUN mkdir -p /app/Data

ENTRYPOINT ["dotnet", "InventoryApi.dll"]
