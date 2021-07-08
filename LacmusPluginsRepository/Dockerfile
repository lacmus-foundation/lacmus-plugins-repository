FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LacmusPluginsRepository.csproj", "LacmusPluginsRepository.csproj"]
RUN dotnet restore "LacmusPluginsRepository.csproj"
COPY . .
RUN dotnet build "LacmusPluginsRepository.csproj" -c Release -o /app/build
RUN dotnet publish "LacmusPluginsRepository.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LacmusPluginsRepository.dll"]
