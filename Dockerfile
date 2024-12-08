# ---------------------------
# Stage 1: Build
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
# Restore all project dependencies
RUN dotnet restore

# Pass user secrets to web api project
# RUN dotnet user-secrets set "JwtSettings:ValidTimeMinutes" "${JWT_VALID_TIME_MINUTES}" --project src/LunaSphere.Api
# RUN dotnet user-secrets set "JwtSettings:Secret" "${JWT_SECRET}" --project src/LunaSphere.Api
# RUN dotnet user-secrets set "JwtSettings:Issuer" "${JWT_ISSUER}" --project src/LunaSphere.Api
# RUN dotnet user-secrets set "JwtSettings:Audience" "${JWT_AUDIENCE}" --project src/LunaSphere.Api
# RUN dotnet user-secrets set "ConnectionStrings:DefaultDbConnection" "${DB_CONNECTION_STRING}" --project src/LunaSphere.Api

# Build web api project
RUN dotnet build src/LunaSphere.Api/LunaSphere.Api.csproj -c Release -o /app/build

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

