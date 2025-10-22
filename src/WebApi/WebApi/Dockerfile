# Dockerfile para .NET 8 WebAPI
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar la solución desde el nivel superior
COPY ../../BookingApp.sln ./

# Copiar proyectos
COPY ../../src/WebApi/*.csproj ./WebApi/
COPY ../../src/Manager/*.csproj ./Manager/
COPY ../../src/Repository/*.csproj ./Repository/
COPY ../../src/Domain/*.csproj ./Domain/

# Restaurar dependencias
RUN dotnet restore BookingApp.sln

# Copiar todo el contenido del proyecto
COPY ../../src ./ 

WORKDIR /src/WebApi
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
