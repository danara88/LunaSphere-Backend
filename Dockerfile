# ---------------------------
# Stage 1: Build
# ---------------------------
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
# Restore web api project
RUN dotnet restore src/LunaSphere.Api/LunaSphere.Api.csproj

# Build web api project
RUN dotnet build src/LunaSphere.Api/LunaSphere.Api.csproj -c Release -o /app/build

# Pubslih web api
RUN dotnet publish src/LunaSphere.Api/LunaSphere.Api.csproj -c Release -o /app/publish --no-restore

# ---------------------------
# Stage 2: Run
# ---------------------------
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
ENV ASPNETCORE_HTTP_PORTS=5001
WORKDIR /app

# Copy the pusblish project folder
COPY --from=build /app/publish .

EXPOSE 5001
ENTRYPOINT ["dotnet","LunaSphere.Api.dll"]

