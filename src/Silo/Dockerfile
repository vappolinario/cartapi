FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY . ./
RUN dotnet restore "src/Silo"
RUN dotnet build "src/Silo" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Silo" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Silo.dll"]
