# Backend API Helm Chart

## Overview
This Helm chart deploys a backend API application to a Kubernetes cluster. The chart is designed for a .NET Core/ASP.NET Core application with built-in health checks and monitoring capabilities.

## Features
- **Health Checks**: Liveness and readiness probes.
- **Resource Limits**: Configurable CPU and memory constraints.
- **Monitoring**: Integrated with Prometheus for metrics scraping.

## Installation
1. Add the Helm repository:
   ```bash
   helm repo add my-repo https://example.com/helm-charts
   helm repo update
   ```
2. Install the backend API:
   ```bash
   helm install backend ./kubernetes/application-helm-chart --namespace backend --create-namespace
   ```

## Configuration
Key configuration options in `values.yaml`:
```yaml
replicaCount: 2
image:
  repository: my-repo/backend-api
  tag: latest
service:
  type: NodePort
  nodePort: 30080
resources:
  limits:
    cpu: 500m
    memory: 512Mi
```