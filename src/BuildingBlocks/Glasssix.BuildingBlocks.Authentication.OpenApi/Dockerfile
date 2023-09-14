FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IdentityAuthServer.Admin.Api/IdentityAuthServer.Admin.Api.csproj", "src/IdentityAuthServer.Admin.Api/"]
COPY ["src/IdentityAuthServer.Admin.EntityFramework.Shared/IdentityAuthServer.Admin.EntityFramework.Shared.csproj", "src/IdentityAuthServer.Admin.EntityFramework.Shared/"]
COPY ["src/IdentityAuthServer.Admin.EntityFramework.SqlServer/IdentityAuthServer.Admin.EntityFramework.SqlServer.csproj", "src/IdentityAuthServer.Admin.EntityFramework.SqlServer/"]
COPY ["src/IdentityAuthServer.Shared/IdentityAuthServer.Shared.csproj", "src/IdentityAuthServer.Shared/"]
COPY ["src/IdentityAuthServer.Admin.EntityFramework.PostgreSQL/IdentityAuthServer.Admin.EntityFramework.PostgreSQL.csproj", "src/IdentityAuthServer.Admin.EntityFramework.PostgreSQL/"]
COPY ["src/IdentityAuthServer.Admin.EntityFramework.MySql/IdentityAuthServer.Admin.EntityFramework.MySql.csproj", "src/IdentityAuthServer.Admin.EntityFramework.MySql/"]
RUN dotnet restore "src/IdentityAuthServer.Admin.Api/IdentityAuthServer.Admin.Api.csproj"
COPY . .
WORKDIR "/src/src/IdentityAuthServer.Admin.Api"
RUN dotnet build "IdentityAuthServer.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityAuthServer.Admin.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "IdentityAuthServer.Admin.Api.dll"]