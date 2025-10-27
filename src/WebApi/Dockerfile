# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar la solución y los proyectos
COPY ["WebApi.sln", "./"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["WebApi/WebApi.csproj", "WebApi/"]

# Restaurar dependencias
RUN dotnet restore WebApi.sln

# Copiar el resto de los archivos
COPY . .

# Publicar
WORKDIR /src/WebApi
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
