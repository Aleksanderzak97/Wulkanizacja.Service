# Zobacz https://aka.ms/customizecontainer, aby dowiedzieć się, jak dostosować kontener debugowania i jak program Visual Studio używa tego pliku Dockerfile do kompilowania obrazów w celu szybszego debugowania.

# Ten etap jest używany podczas uruchamiania z programu VS w trybie szybkim (domyślnie dla konfiguracji debugowania)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app


# Ten etap służy do kompilowania projektu usługi
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Wulkanizacja.Service.sln", "./"]

COPY ["Wulkanizacja.Service.Api/Wulkanizacja.Service.Api.csproj", "Wulkanizacja.Service.Api/"]
COPY ["Wulkanizacja.Service.Infrastructure/Wulkanizacja.Service.Infrastructure.csproj", "Wulkanizacja.Service.Infrastructure/"]
COPY ["Wulkanizacja.Service.Application/Wulkanizacja.Service.Application.csproj", "Wulkanizacja.Service.Application/"]
COPY ["Wulkanizacja.Service.Core/Wulkanizacja.Service.Core.csproj", "Wulkanizacja.Service.Core/"]

RUN dotnet restore "Wulkanizacja.Service.Api/Wulkanizacja.Service.Api.csproj"
COPY . .
RUN dotnet build "Wulkanizacja.Service.Api/Wulkanizacja.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Ten etap służy do publikowania projektu usługi do skopiowania do etapu końcowego
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Wulkanizacja.Service.Api/Wulkanizacja.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Ten etap jest używany w środowisku produkcyjnym lub w przypadku uruchamiania z programu VS w trybie regularnym (domyślnie, gdy nie jest używana konfiguracja debugowania)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wulkanizacja.Service.Api.dll"]