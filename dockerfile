# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos los proyectos primero (para aprovechar el cache de docker)
COPY ["CerebroXMen.API/CerebroXMen.API.csproj", "CerebroXMen.API/"]
COPY ["CerebroXMen.Application/CerebroXMen.Application.csproj", "CerebroXMen.Application/"]
COPY ["CerebroXMen.Domain/CerebroXMen.Domain.csproj", "CerebroXMen.Domain/"]
COPY ["CerebroXMen.Infrastructure/CerebroXMen.Infrastructure.csproj", "CerebroXMen.Infrastructure/"]
COPY ["CerebroXMen-Refactor.sln", "."]

# Restauramos usando la solución
RUN dotnet restore "CerebroXMen-Refactor.sln"

# Ahora copiamos todo lo demás
COPY . .

# Publicamos el proyecto principal
RUN dotnet publish "CerebroXMen.API/CerebroXMen.API.csproj" -c Release -o /app/publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CerebroXMen.API.dll"]