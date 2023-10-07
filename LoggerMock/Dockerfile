FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LoggerMock/LoggerMock.csproj", "LoggerMock/"]
RUN dotnet restore "LoggerMock/LoggerMock.csproj"
COPY . .
WORKDIR "/src/LoggerMock"
RUN dotnet build "LoggerMock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoggerMock.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoggerMock.dll"]
