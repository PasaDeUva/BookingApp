# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de los proyectos individuales
COPY ./src/Application/*.csproj ./Application/
COPY ./src/Infrastructure/*.csproj ./Infrastructure/
COPY ./src/WebApi/*.csproj ./WebApi/

# Restaurar dependencias usando la solución
WORKDIR /src/WebApi
COPY ./src/WebApi/WebApi.sln ./
RUN dotnet restore WebApi.sln

# Copiar todo el código fuente
COPY ./src ./ 

# Publicar
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
