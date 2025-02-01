FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5050

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ManagementServer/ManagementServer.csproj", "ManagementServer/"]
COPY ["RtspServer/RtspServer.csproj", "RtspServer/"]
RUN dotnet restore "ManagementServer/ManagementServer.csproj"
COPY . .
WORKDIR "/src/ManagementServer"
RUN dotnet build "ManagementServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ManagementServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ManagementServer.dll"]
