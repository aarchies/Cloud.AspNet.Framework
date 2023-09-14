#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["test/Redis.Test/Redis.Test.csproj", "test/Redis.Test/"]
RUN dotnet restore "test/Redis.Test/Redis.Test.csproj"
COPY . .
WORKDIR "/src/test/Redis.Test"
RUN dotnet build "Redis.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Redis.Test.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Redis.Test.dll"]