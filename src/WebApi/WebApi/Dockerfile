# Dockerfile para .NET 8 WebAPI
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solución y proyectos
COPY *.sln .
COPY WebApi/*.csproj ./WebApi/
COPY Manager/*.csproj ./Manager/
COPY Repository/*.csproj ./Repository/
COPY Domain/*.csproj ./Domain/

# Restaurar paquetes
RUN dotnet restore

# Copiar todo el contenido del proyecto
COPY . .

WORKDIR /src/WebApi
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
