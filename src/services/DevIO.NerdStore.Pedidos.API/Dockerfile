# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./services/DevIO.NerdStore.Pedidos.API/DevIO.NerdStore.Pedidos.API.csproj", "services/DevIO.NerdStore.Pedidos.API/"]
COPY ["./building blocks/DevIO.NerdStore.Core/DevIO.NerdStore.Core.csproj", "building blocks/DevIO.NerdStore.Core/"]
COPY ["./building blocks/DevIO.NerdStore.MessageBus/DevIO.NerdStore.MessageBus.csproj", "building blocks/DevIO.NerdStore.MessageBus/"]
COPY ["./building blocks/DevIO.NerdStore.WebAPI.Core/DevIO.NerdStore.WebAPI.Core.csproj", "building blocks/DevIO.NerdStore.WebAPI.Core/"]
COPY ["./services/DevIO.NerdStore.Pedidos.Infra/DevIO.NerdStore.Pedidos.Infra.csproj", "services/DevIO.NerdStore.Pedidos.Infra/"]
COPY ["./services/DevIO.NerdStore.Pedidos.Domain/DevIO.NerdStore.Pedidos.Domain.csproj", "services/DevIO.NerdStore.Pedidos.Domain/"]
RUN dotnet restore "services/DevIO.NerdStore.Pedidos.API/DevIO.NerdStore.Pedidos.API.csproj"
COPY . .
WORKDIR "services/DevIO.NerdStore.Pedidos.API"
RUN dotnet build "DevIO.NerdStore.Pedidos.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DevIO.NerdStore.Pedidos.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevIO.NerdStore.Pedidos.API.dll"]