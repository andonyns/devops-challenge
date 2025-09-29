# Backend

Build docker image:


Run:
```bash
docker run -p 8080:80 --name my-store -d store-api
```

Sample application that has CRUD (Create, Read, Update and Delete) operations for adding Items.
This is a .NET 8 application, this can be used from Visual Studio, or a terminal by running:

```bash
    dotnet build
    dotnet run
```

This application uses Docker and has integration with Grafana Loki, therefore, the recommended approach 
is to use the docker-compose.yaml file.

In order to use, build the image with:
```bash
docker build . -t store-api
```

When done, setup the application by running:
```bash
docker compose up -d
```