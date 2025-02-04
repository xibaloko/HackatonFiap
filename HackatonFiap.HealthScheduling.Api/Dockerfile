FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 🛠️ Instala o `dotnet-ef`
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY ["HackatonFiap.HealthScheduling.Api/HackatonFiap.HealthScheduling.Api.csproj", "HackatonFiap.HealthScheduling.Api/"]
COPY ["HackatonFiap.HealthScheduling.Application/HackatonFiap.HealthScheduling.Application.csproj", "HackatonFiap.HealthScheduling.Application/"]
COPY ["HackatonFiap.HealthScheduling.Domain/HackatonFiap.HealthScheduling.Domain.csproj", "HackatonFiap.HealthScheduling.Domain/"]
COPY ["HackatonFiap.HealthScheduling.Infrastructure.SqlServer/HackatonFiap.HealthScheduling.Infrastructure.SqlServer.csproj", "HackatonFiap.HealthScheduling.Infrastructure.SqlServer/"]
COPY ["HackatonFiap.HealthScheduling.Infrastructure.RabbitMq/HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.csproj", "HackatonFiap.HealthScheduling.Infrastructure.RabbitMq/"]

RUN dotnet restore "HackatonFiap.HealthScheduling.Api/HackatonFiap.HealthScheduling.Api.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.HealthScheduling.Api"
RUN dotnet build "HackatonFiap.HealthScheduling.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HackatonFiap.HealthScheduling.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Copia o `dotnet-ef` para o contêiner final
RUN cp -r /root/.dotnet /app/dotnet-tools

# Etapa final
FROM base AS final
WORKDIR /app

# Copia os arquivos publicados
COPY --from=publish /app/publish .

# Adiciona `dotnet-ef` ao PATH no contêiner final
COPY --from=publish /app/dotnet-tools /root/.dotnet
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "HackatonFiap.HealthScheduling.Api.dll"]