# FitnessTracker API Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/FitnessTracker.API/FitnessTracker.API.csproj", "src/FitnessTracker.API/"]
COPY ["src/FitnessTracker.Core/FitnessTracker.Core.csproj", "src/FitnessTracker.Core/"]
COPY ["src/FitnessTracker.Infrastructure/FitnessTracker.Infrastructure.csproj", "src/FitnessTracker.Infrastructure/"]
RUN dotnet restore "src/FitnessTracker.API/FitnessTracker.API.csproj"
COPY . .
WORKDIR /src/src/FitnessTracker.API
RUN dotnet publish "FitnessTracker.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FitnessTracker.API.dll"]
