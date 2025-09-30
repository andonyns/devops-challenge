# Backend Application

## Overview
This is a .NET 8 REST API application that provides CRUD operations for managing items. The application is containerized and integrates with the logging stack (Promtail, Loki, Grafana).

## Features
- **CRUD Operations**: Create, Read, Update, Delete items.
- **Health Checks**: Built-in `/health` and `/ready` endpoints.
- **Logging**: Integrated with Grafana Loki for centralized logging.

## Prerequisites
1. Install Docker.
2. Install .NET SDK 8.

## Usage
### Local Development
1. Build and run the application:
   ```bash
   dotnet build
   dotnet run
   ```
2. Access the API at `http://localhost:5000`.

### Docker
1. Build the Docker image:
   ```bash
   docker build . -t store-api
   ```
2. Run the container:
   ```bash
   docker run -p 9000:80 --name my-store -d store-api
   ```

### Docker Compose
1. Start the application using Docker Compose:
   ```bash
   docker compose up -d
   ```