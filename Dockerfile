FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5006

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/HotelUp.Kitchen.API/HotelUp.Kitchen.API.csproj", "Src/HotelUp.Kitchen.API/"]
COPY ["Src/HotelUp.Kitchen.Services/HotelUp.Kitchen.Services.csproj", "Src/HotelUp.Kitchen.Services/"]
COPY ["Src/HotelUp.Kitchen.Persistence/HotelUp.Kitchen.Persistence.csproj", "Src/HotelUp.Kitchen.Persistence/"]
COPY ["Shared/HotelUp.Kitchen.Shared/HotelUp.Kitchen.Shared.csproj", "Shared/HotelUp.Kitchen.Shared/"]
RUN dotnet restore "Src/HotelUp.Kitchen.API/HotelUp.Kitchen.API.csproj"
COPY . .
WORKDIR "/src/Src/HotelUp.Kitchen.API"
RUN dotnet build "HotelUp.Kitchen.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HotelUp.Kitchen.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl --silent --fail http://localhost:5006/api/kitchen/_health || exit 1
ENTRYPOINT ["dotnet", "HotelUp.Kitchen.API.dll"]
