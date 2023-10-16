FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Sql/Sql.csproj", "src/Sql/"]
RUN dotnet restore "src/Sql/Sql.csproj"
COPY . .
WORKDIR "/src/src/Sql"
RUN dotnet build "Sql.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sql.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sql.dll"]
