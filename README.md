# Welcome to LunaSphere (backend) 🤘🏼

## Docker commands

- If you want to build the image in your local, execute following command in the project root:

```
docker build  -t daranda88/luna-sphere-backend:1.0.0 .
```

- If you want to build the image with included platforms and push to docker hub execute the following command in the project root:

```
docker buildx build --platform linux/amd64,linux/arm64 -t daranda88/luna-sphere-backend:1.0.0 --push .
```
