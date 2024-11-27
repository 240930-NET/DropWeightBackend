FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /app

COPY *.sln ./
COPY DropWeightBackend.Api /app/DropWeightBackend.Api 
COPY DropWeightBackend.Domain /app/DropWeightBackend.Domain 
COPY DropWeightBackend.Infrastructure /app/DropWeightBackend.Infrastructure
COPY DropWeightBackend.Tests /app/DropWeightBackend.Tests 

# Build
RUN dotnet build -c Release

# Publish
FROM build-env AS publish-env
WORKDIR /app
RUN dotnet publish DropWeightBackend.Api/DropWeightBackend.Api.csproj -c Release -o dist

# Make new layer
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run

# Ask aspnet to serve over port 80... please
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_URLS=http://*:80
WORKDIR /app
COPY --from=publish-env /app/dist .


#Startup command
ENTRYPOINT ["dotnet", "DropWeightBackend.Api.dll"]
