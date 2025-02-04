FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HackatonFiap.Identity.Api/HackatonFiap.Identity.Api.csproj", "HackatonFiap.Identity.Api/"]
COPY ["HackatonFiap.Identity.Application/HackatonFiap.Identity.Application.csproj", "HackatonFiap.Identity.Application/"]
COPY ["HackatonFiap.Identity.Domain/HackatonFiap.Identity.Domain.csproj", "HackatonFiap.Identity.Domain/"]
COPY ["HackatonFiap.Identity.Infrastructure.SqlServer/HackatonFiap.Identity.Infrastructure.SqlServer.csproj", "HackatonFiap.Identity.Infrastructure.SqlServer/"]
RUN dotnet restore "HackatonFiap.Identity.Api/HackatonFiap.Identity.Api.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.Identity.Api"
RUN dotnet build "HackatonFiap.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HackatonFiap.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.Identity.Api.dll"]
