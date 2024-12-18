# ---------------------------
# Stage 1: Build
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
# Restore all project dependencies
RUN dotnet restore

# Build web api project
RUN dotnet build src/LunaSphere.Api/LunaSphere.Api.csproj -c Release -o /app/build

# Run unit tests
RUN dotnet test

# Pubslih web api
RUN dotnet publish src/LunaSphere.Api/LunaSphere.Api.csproj -c Release -o /app/publish --no-restore

# ---------------------------
# Stage 2: Run
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app

# Copy the pusblish project folder
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet","LunaSphere.Api.dll"]

