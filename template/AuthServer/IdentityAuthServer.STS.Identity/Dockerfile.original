FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IdentityAuthServer.STS.Identity/IdentityAuthServer.STS.Identity.csproj", "src/IdentityAuthServer.STS.Identity/"]
COPY ["src/IdentityAuthServer.Admin.EntityFramework.Shared/IdentityAuthServer.Admin.EntityFramework.Shared.csproj", "src/IdentityAuthServer.Admin.EntityFramework.Shared/"]
COPY ["src/IdentityAuthServer.Shared/IdentityAuthServer.Shared.csproj", "src/IdentityAuthServer.Shared/"]
RUN dotnet restore "src/IdentityAuthServer.STS.Identity/IdentityAuthServer.STS.Identity.csproj"
COPY . .
WORKDIR "/src/src/IdentityAuthServer.STS.Identity"
RUN dotnet build "IdentityAuthServer.STS.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityAuthServer.STS.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "IdentityAuthServer.STS.Identity.dll"]