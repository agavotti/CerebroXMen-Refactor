# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de solución
COPY ["CerebroXMen-Refactor.sln", "."]
COPY ["CerebroXMen.API/CerebroXMen.API.csproj", "CerebroXMen.API/"]
COPY ["CerebroXMen.Application/CerebroXMen.Application.csproj", "CerebroXMen.Application/"]
COPY ["CerebroXMen.Domain/CerebroXMen.Domain.csproj", "CerebroXMen.Domain/"]
COPY ["CerebroXMen.Infrastructure/CerebroXMen.Infrastructure.csproj", "CerebroXMen.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "CerebroXMen-Refactor.sln"

# Copiar el resto del código
COPY . .

# Compilar y publicar en modo release
WORKDIR "/src/CerebroXMen.API"
RUN dotnet publish -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Puerto expuesto (ajustalo si tu app usa otro)
EXPOSE 80

# Comando para correr la app
ENTRYPOINT ["dotnet", "CerebroXMen.API.dll"]