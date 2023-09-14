#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["template/AuthServer/IdentityAuthServer.Admin.Api/IdentityAuthServer.Admin.Api.csproj", "template/AuthServer/IdentityAuthServer.Admin.Api/"]
COPY ["src/Contrib/Glasssix.Contrib.Authentication.EntityFramework.MySql/Glasssix.Contrib.Authentication.EntityFramework.MySql.csproj", "src/Contrib/Glasssix.Contrib.Authentication.EntityFramework.MySql/"]
COPY ["src/Contrib/Glasssix.Contrib.Authentication.EntityFramework.Shared/Glasssix.Contrib.Authentication.EntityFramework.Shared.csproj", "src/Contrib/Glasssix.Contrib.Authentication.EntityFramework.Shared/"]
COPY ["src/Contrib/Glasssix.Contrib.Authentication.Shared/Glasssix.Contrib.Authentication.Shared.csproj", "src/Contrib/Glasssix.Contrib.Authentication.Shared/"]
RUN dotnet restore "template/AuthServer/IdentityAuthServer.Admin.Api/IdentityAuthServer.Admin.Api.csproj"
COPY . .
WORKDIR "/src/template/AuthServer/IdentityAuthServer.Admin.Api"
RUN dotnet build "IdentityAuthServer.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityAuthServer.Admin.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityAuthServer.Admin.Api.dll"]