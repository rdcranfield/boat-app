FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["boat-app-v2/boat-app-v2.csproj", "boat-app-v2/"]
RUN dotnet restore "boat-app-v2/boat-app-v2.csproj"
COPY . .
WORKDIR "/src/boat-app-v2"
RUN dotnet build "boat-app-v2.csproj" -c Release -o /app/build
RUN dotnet publish "boat-app-v2.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/publish .
#COPY --from=build /https/server.pfx /https/se

EXPOSE 8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_VERSION=8.0.1
ENV ASPNET_VERSION=8.0.1
ENV DOTNET_URLS=http://+:5031
ENV ASPNETCORE_URLS=http://+:5031
ENV ASPNETCORE_HTTPS_PORT=https://+:5032
ENV  ASPNETCORE_ENVIRONMENT: Development

ENTRYPOINT ["dotnet", "boat-app-v2.dll"]
