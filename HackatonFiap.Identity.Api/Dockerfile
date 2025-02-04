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

COPY ["HackatonFiap.Identity.Api/HackatonFiap.Identity.Api.csproj", "HackatonFiap.Identity.Api/"]
COPY ["HackatonFiap.Identity.Application/HackatonFiap.Identity.Application.csproj", "HackatonFiap.Identity.Application/"]
COPY ["HackatonFiap.Identity.Domain/HackatonFiap.Identity.Domain.csproj", "HackatonFiap.Identity.Domain/"]
COPY ["HackatonFiap.Identity.Infrastructure.SqlServer/HackatonFiap.Identity.Infrastructure.SqlServer.csproj", "HackatonFiap.Identity.Infrastructure.SqlServer/"]

RUN dotnet restore "HackatonFiap.Identity.Api/HackatonFiap.Identity.Api.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.Identity.Api"
RUN dotnet build "HackatonFiap.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HackatonFiap.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

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

ENTRYPOINT ["dotnet", "HackatonFiap.Identity.Api.dll"]