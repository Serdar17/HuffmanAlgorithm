FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Archiver.Web/Archiver.Web.csproj", "Archiver.Web/"]
RUN dotnet restore "Archiver.Web/Archiver.Web.csproj"
COPY . .
WORKDIR "/src/Archiver.Web"
RUN dotnet build "Archiver.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Archiver.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Archiver.Web.dll"]
