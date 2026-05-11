# Base Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

EXPOSE 8080

# Build Image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

#Copy csproj files
COPY ["Identity.API/Identity.API.csproj", "Identity.API/"]
COPY ["Identity.Application/Identity.Application.csproj", "Identity.Application/"]
COPY ["Identity.Domain/Identity.Domain.csproj", "Identity.Domain/"]
COPY ["Identity.Infrastructure/Identity.Infrastructure.csproj", "Identity.Infrastructure/"]

# Restore packages
RUN dotnet restore "Identity.API/Identity.API.csproj"

# Copy everything
COPY . .

WORKDIR "/src/Identity.API"

# Build project
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish

RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish

# Final Runtime
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Identity.API.dll"]