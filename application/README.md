# Backend

Build docker image:
```bash
docker build . -t store-api
```

Run:
```bash
docker run -p 8080:80 --name my-store -d store-api
```