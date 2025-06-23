FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LeawareTest.API/LeawareTest.API.csproj", "LeawareTest.API/"]
RUN dotnet restore "LeawareTest.API/LeawareTest.API.csproj"
COPY . .
WORKDIR "/src/LeawareTest.API"
RUN dotnet build "./LeawareTest.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LeawareTest.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY LeawareTest.API/wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

USER $APP_UID

ENTRYPOINT ["/wait-for-it.sh", "db:3306", "--", "dotnet", "LeawareTest.API.dll"]
