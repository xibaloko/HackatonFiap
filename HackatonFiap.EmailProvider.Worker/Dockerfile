FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HackatonFiap.EmailProvider.Worker/HackatonFiap.EmailProvider.Worker.csproj", "HackatonFiap.EmailProvider.Worker/"]
RUN dotnet restore "HackatonFiap.EmailProvider.Worker/HackatonFiap.EmailProvider.Worker.csproj"
COPY . .
WORKDIR "/src/HackatonFiap.EmailProvider.Worker"
RUN dotnet build "HackatonFiap.EmailProvider.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HackatonFiap.EmailProvider.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackatonFiap.EmailProvider.Worker.dll"]
