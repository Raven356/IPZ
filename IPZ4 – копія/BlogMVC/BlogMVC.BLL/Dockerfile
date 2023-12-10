# Dockerfile.BlogMVC.BLL
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlogMVC.BLL/BlogMVC.BLL.csproj", "BlogMVC.BLL/"]
RUN dotnet restore "./BlogMVC.BLL/BlogMVC.BLL.csproj"
COPY . .
WORKDIR "/src/BlogMVC.BLL"
RUN dotnet build "./BlogMVC.BLL.csproj" -c $BUILD_CONFIGURATION -o /app/build

