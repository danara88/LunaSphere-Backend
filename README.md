# Welcome to LunaSphere (backend) 🤘🏼🎧

## Table of Contents 📖

- [Branch and commits naming convention](#branch-and-commits-naming-convention-)
- [Run the project in your local environment](#run-the-project-in-your-local-environment-)
- [Configure project user secrets](#configure-project-user-secrets-)
- [Docker configuration](#docker-configuration-)
- [API endpoints](#api-endpoints-)

## Branch and commits naming convention 🧐

For branch naming follow this structure:
**feat/LS-OOOO-your-branch-description**

For commit naming follow this structure:
**feat(LS-0000): your commit message**

---

## Run the project in your local environment 📀

To run the project locally, follow these instructions:

⚠️ **Caution:** Before running the project, make sure to complete the user secrets setup as outlined in the next section.

1. Execute the following command to build the project in the project root directory:

```bash
$ dotnet build
```

2. Run the following command in the project root:

```bash
$ dotnet run -p ./src/LunaSphere.Api
```

---

## Configure project user secrets 💻

To run the project locally, you need to configure the required user secrets:

1. If you haven't initialized your user secrets yet, run the following command in the project root:

```bash
$ dotnet user-secrets init -p ./src/LunaSphere.Api
```

2. Set up the **database connection** string user secrets cofiguration by running the following commands in the project root directory:

```bash
$ dotnet user-secrets set "ConnectionStrings:DefaultDbConnection" "YourDbConnectionStringHere" -p ./src/LunaSphere.Api
```

3. Set up the **JWT** user secrets configuration by running the following commands in the project root directory:

```bash
$ dotnet user-secrets set "JwtSettings:ValidTimeMinutes" "[JwtValidTimeMinutes]" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "JwtSettings:Secret" "[YourSecr3tK3y]" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "JwtSettings:Issuer" "http://localhost:5063" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "JwtSettings:Audience" "http://localhost:4200" -p ./src/LunaSphere.Api
```

4. Set up the **Google API authentication** user secrets configuration by running the following commands in the project root directory:

```bash
$ dotnet user-secrets set "GoogleAuthSettings:GoogleId" "[YourGoogleIdHere]" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "GoogleAuthSettings:Secret" "[YourGoogleApiSecret]" -p ./src/LunaSphere.Api
```

5. Set up the **SMTP** user secrets configuration by running the following commands in the project root directory:

```bash
$ dotnet user-secrets set "SmtpSettings:UserName" "[YourSmtpUserName]" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "SmtpSettings:Server" "[YourSmtpServerName]" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "SmtpSettings:SenderName" "[YourSmtpSernderName]" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "SmtpSettings:SenderEmail" "[YourSmtpSernderEmail]" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "SmtpSettings:Port" "[YourSmtpPort]" -p ./src/LunaSphere.Api

$ dotnet user-secrets set "SmtpSettings:Password" "[YourSmtpPassword]" -p ./src/LunaSphere.Api
```

---

## Docker configuration 📝

If you want to build the image locally, run the following command in the project root:

```bash
$ docker build  -t daranda88/luna-sphere-backend:1.0.0 .
```

To build the image with included platforms and push it to Docker Hub, run the following command in the project root:

```bash
$ docker buildx build --platform linux/amd64,linux/arm64 -t daranda88/luna-sphere-backend:1.0.0 --push .
```

---

## API endpoints 🛜

Authentication Module Endpoints:
| Endpoint | Description | Version |
| -------- | ----------- | ------- |
| /api/v1/auth/register | Responsible for registering users and business accounts. | 1.0|
| /api/v1/auth/login | Handles user and business account sign-ins. | 1.0|
| /api/v1/auth/google-signin | Handles user sign-ins using Google API authentication (excluding business accounts). | 1.0|
| /api/v1/auth/refresh-token | Responsible for returning a valid refresh token. | 1.0|
| /api/v1/auth/user-eligible-for-verification | Checks if a user account is eligible for verification. | 1.0|
| /api/v1/auth/verify-verification-code | Verifies if the verification code is valid. | 1.0|
| /api/v1/auth/resend-verification-code | Resends a valid verification code to the user's email. | 1.0|
