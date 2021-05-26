FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY NuGet.Config NuGet.Config
COPY TextService/TextService.csproj TextService/TextService.csproj
COPY TextService.Data/TextService.Data.csproj TextService.Data/TextService.Data.csproj
RUN dotnet restore TextService/TextService.csproj
COPY . .
WORKDIR /src
RUN dotnet build TextService/TextService.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish  TextService/TextService.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
    ENTRYPOINT ["dotnet", "TextService.dll"]