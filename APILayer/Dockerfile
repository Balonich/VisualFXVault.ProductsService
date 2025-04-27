FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["APILayer/APILayer.csproj", "APILayer/"]
RUN dotnet restore "APILayer/APILayer.csproj"
COPY . .
WORKDIR "/src/APILayer"
RUN dotnet build "APILayer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "APILayer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV SQLSERVER_HOST=localhost
ENV SQLSERVER_USER=sa
ENV SQLSERVER_PASSWORD=yourStrong(!)Password
ENTRYPOINT ["dotnet", "APILayer.dll"]