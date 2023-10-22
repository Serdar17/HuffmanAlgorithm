FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Archiver.Api/Archiver.Api.csproj", "Archiver.Api/"]
RUN dotnet restore "Archiver.Api/Archiver.Api.csproj"
COPY . .
WORKDIR "/src/Archiver.Api"
RUN dotnet build "Archiver.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Archiver.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Archiver.Api.dll"]
