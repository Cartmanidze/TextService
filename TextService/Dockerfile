FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ./TextService/TextService ./TextService/TextService
COPY ./TextService.Data ./TextService.Data
WORKDIR ./TextService/TextService
RUN dotnet restore "TextService.csproj"
RUN dotnet build "TextService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish  "TextService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
    ENTRYPOINT ["dotnet", "TextService.dll"]