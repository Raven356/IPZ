# Dockerfile.BlogMVC.DAL
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlogMVC.DAL/BlogMVC.DAL.csproj", "BlogMVC.DAL/"]
RUN dotnet restore "./BlogMVC.DAL/BlogMVC.DAL.csproj"
COPY . .
WORKDIR "/src/BlogMVC.DAL"
RUN dotnet build "./BlogMVC.DAL.csproj" -c $BUILD_CONFIGURATION -o /app/build
