FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
USER root
RUN apt-get update && \
    apt-get install -y afl
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["RtpPayloadGenerator/RtpPayloadGenerator.csproj", "RtpPayloadGenerator/"]
COPY ["AflTarget/AflTarget.csproj", "AflTarget/"]
RUN dotnet restore "RtpPayloadGenerator/RtpPayloadGenerator.csproj"
RUN dotnet restore "AflTarget/AflTarget.csproj"

COPY . .
WORKDIR "/src/RtpPayloadGenerator"
RUN dotnet build "RtpPayloadGenerator.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/AflTarget"
RUN dotnet build "AflTarget.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
WORKDIR "/src/RtpPayloadGenerator"
RUN dotnet publish "RtpPayloadGenerator.csproj" -c $BUILD_CONFIGURATION -o /app/publish/RtpPayloadGenerator /p:UseAppHost=false

WORKDIR "/src/AflTarget"
RUN dotnet publish "AflTarget.csproj" -c $BUILD_CONFIGURATION -o /app/publish/AflTarget /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish/RtpPayloadGenerator ./RtpPayloadGenerator
COPY --from=publish /app/publish/AflTarget ./AflTarget
WORKDIR /app/RtpPayloadGenerator


ENTRYPOINT ["dotnet", "RtpPayloadGenerator.dll"]
