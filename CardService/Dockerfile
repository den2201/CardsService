FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CardService/CardService.csproj", "CardService/"]
COPY ["SharedEntities/SharedEntities.csproj", "SharedEntities/"]
RUN dotnet restore "CardService/CardService.csproj"
COPY . .
WORKDIR "/src/CardService"
RUN dotnet build "CardService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CardService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CardService.dll"]
