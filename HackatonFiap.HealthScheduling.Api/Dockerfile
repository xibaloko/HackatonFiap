FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HackatonFiap.HealthScheduling.Api/HackatonFiap.HealthScheduling.Api.csproj", "HackatonFiap.HealthScheduling.Api/"]
COPY ["HackatonFiap.HealthScheduling.Application/HackatonFiap.HealthScheduling.Application.csproj", "HackatonFiap.HealthScheduling.Application/"]
COPY ["HackatonFiap.HealthScheduling.Domain/HackatonFiap.HealthScheduling.Domain.csproj", "HackatonFiap.HealthScheduling.Domain/"]
COPY ["HackatonFiap.HealthScheduling.Infrastructure.SqlServer/HackatonFiap.HealthScheduling.Infrastructure.SqlServer.csproj", "HackatonFiap.HealthScheduling.Infrastructure.SqlServer/"]
RUN dotnet restore "HackatonFiap.HealthScheduling.Api/HackatonFiap.HealthScheduling.Api.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.HealthScheduling.Api"
RUN dotnet build "HackatonFiap.HealthScheduling.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HackatonFiap.HealthScheduling.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.HealthScheduling.Api.dll"]