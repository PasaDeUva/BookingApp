FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar sln
COPY WebApi.sln ./

# Copiar todos los proyectos
COPY Application/ Application/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/
COPY WebApi/ WebApi/

# Restaurar dependencias
RUN dotnet restore WebApi.sln

# Publicar
WORKDIR /src/WebApi
RUN dotnet publish WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false
# NOTA: ya no usamos --no-restore

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
