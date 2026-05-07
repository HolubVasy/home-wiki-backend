FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/ .
RUN dotnet restore home-wiki-backend.WebApi/home-wiki-backend.WebApi.csproj
RUN dotnet publish home-wiki-backend.WebApi/home-wiki-backend.WebApi.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "home-wiki-backend.WebApi.dll"]
