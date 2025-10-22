# Etapa base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar la solución y el proyecto
COPY WebApi.sln ./
COPY WebApi.csproj ./

# Restaurar dependencias
RUN dotnet restore WebApi.csproj

# Copiar todo el código
COPY . ./

# Compilar y publicar
RUN dotnet publish WebApi.csproj -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
