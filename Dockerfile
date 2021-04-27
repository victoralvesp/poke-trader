#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN apt-get install --yes curl
RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs
WORKDIR "/src/PokeTrader.WebApp"
COPY ["src/PokeTrader.WebApp/Spa/package.json", "Spa/"]
COPY ["src/PokeTrader.WebApp/Spa/package-lock.json", "Spa/"]
WORKDIR "/src/PokeTrader.WebApp/Spa"
RUN npm install
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/PokeTrader.WebApp/PokeTrader.WebApp.csproj", "PokeTrader.WebApp/"]
COPY ["src/PokeTrader.DTO/PokeTrader.DTO.csproj", "PokeTrader.DTO/"]
COPY ["src/PokeTrader.Core/PokeTrader.Core.csproj", "PokeTrader.Core/"]
COPY ["src/PokeTrader.Data/PokeTrader.Data.csproj", "PokeTrader.Data/"]
COPY "./src" .
RUN dotnet restore "PokeTrader.WebApp/PokeTrader.WebApp.csproj"
RUN dotnet build "PokeTrader.WebApp/PokeTrader.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src
RUN dotnet publish "PokeTrader.WebApp/PokeTrader.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokeTrader.WebApp.dll"]