FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HybridCacheWithNet9/HybridCacheWithNet9.csproj", "HybridCacheWithNet9/"]
RUN dotnet restore "HybridCacheWithNet9/HybridCacheWithNet9.csproj"
COPY . .
WORKDIR "/src/HybridCacheWithNet9"
RUN dotnet build "HybridCacheWithNet9.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HybridCacheWithNet9.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HybridCacheWithNet9.dll"]
