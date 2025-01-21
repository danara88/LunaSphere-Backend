# Welcome to LunaSphere (backend) 🤘🏼

# Configure local user secrets

- To run your local, you will need to set up some required user secrets:

If you don't have initialized yet your user secrets, execute the following in the project root.

```
dotnet user-secrets init -p ./src/LunaSphere.Api
```

```
dotnet user-secrets set "JwtSettings:ValidTimeMinutes" "[JwtValidTimeMinutes]" -p ./src/LunaSphere.Api
```

```
dotnet user-secrets set "JwtSettings:Secret" "[YourSecr3tK3y]" -p ./src/LunaSphere.Api
```

```
dotnet user-secrets set "JwtSettings:Issuer" "http://localhost:5063" -p ./src/LunaSphere.Api
```

```
dotnet user-secrets set "JwtSettings:Audience" "http://localhost:4200" -p ./src/LunaSphere.Api
```

```
dotnet user-secrets set "GoogleAuthSettings:GoogleId" "[YourGoogleIdHere]" -p ./src/LunaSphere.Api
```

```
dotnet user-secrets set "GoogleAuthSettings:Secret" "[YourGoogleApiSecret]" -p ./src/LunaSphere.Api
```

```
dotnet user-secrets set "ConnectionStrings:DefaultDbConnection" "YourDbConnectionStringHere" -p ./src/LunaSphere.Api
```

## Docker commands

- If you want to build the image in your local, execute following command in the project root:

```
docker build  -t daranda88/luna-sphere-backend:1.0.0 .
```

- If you want to build the image with included platforms and push to docker hub execute the following command in the project root:

```
docker buildx build --platform linux/amd64,linux/arm64 -t daranda88/luna-sphere-backend:1.0.0 --push .
```
