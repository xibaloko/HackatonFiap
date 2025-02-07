FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HackatonFiap.Tests/HackatonFiap.Tests.csproj", "HackatonFiap.Tests/"]
COPY ["HackatonFiap.HealthScheduling.Api/HackatonFiap.HealthScheduling.Api.csproj", "HackatonFiap.HealthScheduling.Api/"]
COPY ["HackatonFiap.HealthScheduling.Application/HackatonFiap.HealthScheduling.Application.csproj", "HackatonFiap.HealthScheduling.Application/"]
COPY ["HackatonFiap.HealthScheduling.Domain/HackatonFiap.HealthScheduling.Domain.csproj", "HackatonFiap.HealthScheduling.Domain/"]
COPY ["HackatonFiap.HealthScheduling.Infrastructure.SqlServer/HackatonFiap.HealthScheduling.Infrastructure.SqlServer.csproj", "HackatonFiap.HealthScheduling.Infrastructure.SqlServer/"]
COPY ["HackatonFiap.HealthScheduling.Infrastructure.RabbitMq/HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.csproj", "HackatonFiap.HealthScheduling.Infrastructure.RabbitMq/"]
COPY ["HackatonFiap.Identity.Application/HackatonFiap.Identity.Application.csproj", "HackatonFiap.Identity.Application/"]
COPY ["HackatonFiap.Identity.Domain/HackatonFiap.Identity.Domain.csproj", "HackatonFiap.Identity.Domain/"]
COPY ["HackatonFiap.Identity.Infrastructure.SqlServer/HackatonFiap.Identity.Infrastructure.SqlServer.csproj", "HackatonFiap.Identity.Infrastructure.SqlServer/"]
RUN dotnet restore "HackatonFiap.Tests/HackatonFiap.Tests.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.Tests"
RUN dotnet build "HackatonFiap.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HackatonFiap.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.Tests.dll"]
