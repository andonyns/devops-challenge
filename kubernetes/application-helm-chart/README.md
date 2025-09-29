# Backend API Helm Chart

This Helm chart deploys a backend API application to a Kubernetes cluster. The chart is designed for a .NET Core/ASP.NET Core application with built-in health checks and monitoring capabilities.

## Overview

- **Chart Name**: backend
- **Chart Version**: 0.2.0
- **App Version**: 1.0
- **Type**: Application

## Components

### Deployment
- **Replicas**: Configurable (default: 1)
- **Container**: .NET Core/ASP.NET Core backend API
- **Health Checks**: Liveness and readiness probes
- **Resource Limits**: Memory and CPU constraints for optimal performance
- **Environment**: Production configuration

### Service
- **Type**: NodePort (default)
- **Ports**: 
  - Service Port: 80
  - Target Port: 9000 (configurable)
  - NodePort: 30080 (external access)

### Monitoring
- **Prometheus Integration**: Configured for metrics scraping
- **Annotations**: Ready for monitoring stack integration

## Installation

### Prerequisites
- Kubernetes cluster (v1.19+)
- Helm 3.x
- Docker registry with the application image

### Quick Install
```bash
# Install with default values
helm install backend ./kubernetes/application-helm-chart

# Install with custom values
helm install backend ./kubernetes/application-helm-chart -f custom-values.yaml

# Install in specific namespace
helm install backend ./kubernetes/application-helm-chart --namespace myapp --create-namespace
```

### Using with Terraform
This chart is designed to work with the Terraform infrastructure modules:
```bash
terraform apply
```

## Configuration

The following table lists the configurable parameters and their default values:

| Parameter | Description | Default |
|-----------|-------------|---------|
| `replicaCount` | Number of backend replicas | `1` |
| `image.repository` | Backend image repository | `store-api` |
| `image.tag` | Backend image tag | `latest` |
| `image.pullPolicy` | Image pull policy | `IfNotPresent` |
| `service.type` | Kubernetes service type | `NodePort` |
| `service.port` | Service port | `80` |
| `service.nodePort` | NodePort for external access | `30080` |
| `service.targetPort` | Container target port | `9000` |

### Example Custom Values

```yaml
# custom-values.yaml
replicaCount: 3

image:
  repository: myregistry/store-api
  tag: "v1.2.3"
  pullPolicy: Always

service:
  type: ClusterIP
  port: 80
  targetPort: 9000

resources:
  requests:
    memory: "128Mi"
    cpu: "500m"
  limits:
    memory: "256Mi"
    cpu: "1000m"
```

## Application Requirements

### Health Endpoints
The application should implement the following endpoints for proper health checking:

- **Liveness Probe**: `GET /health`
  - Should return HTTP 200 when the application is running
  - Used to determine if the container should be restarted

- **Readiness Probe**: `GET /ready`
  - Should return HTTP 200 when the application is ready to serve traffic
  - Used to determine if the pod should receive traffic

### Monitoring Endpoints
- **Metrics**: `GET /metrics` (port 9000)
  - Should expose Prometheus-compatible metrics
  - Used for application monitoring and alerting

### Environment Configuration
The application is configured with:
- `ASPNETCORE_ENVIRONMENT=Production`
- `ASPNETCORE_URLS=http://+:80`

## Accessing the Application

### Local Development (Minikube)
```bash
# Get Minikube IP
minikube ip

# Access application
curl http://$(minikube ip):30080

# Or open in browser
open http://$(minikube ip):30080
```

### Port Forwarding
```bash
# Forward local port to service
kubectl port-forward service/backend 9000:80

# Access via localhost
curl http://localhost:9000
```

### Service Discovery
Within the cluster, the service is available at:
- Service Name: `backend` (or custom name if overridden)
- Port: `80`
- URL: `http://backend:80`

## Monitoring and Observability

### Prometheus Integration
The deployment includes annotations for Prometheus metrics collection:
```yaml
annotations:
  prometheus.io/scrape: "true"
  prometheus.io/port: "9000"
  prometheus.io/path: "/metrics"
```

### Logging
- Application logs are automatically collected by the logging infrastructure
- Logs are available in the Grafana dashboard when the logging module is deployed
- Use structured logging for better observability

### Resource Monitoring
Default resource configuration:
- **Requests**: 64Mi memory, 250m CPU
- **Limits**: 128Mi memory, 500m CPU

## Upgrading

### Rolling Updates
```bash
# Upgrade with new image tag
helm upgrade backend ./kubernetes/application-helm-chart --set image.tag=v1.2.3

# Upgrade with new values
helm upgrade backend ./kubernetes/application-helm-chart -f new-values.yaml
```

### Rollback
```bash
# List releases
helm history backend

# Rollback to previous version
helm rollback backend 1
```

## Uninstalling

```bash
# Uninstall the release
helm uninstall backend

# Uninstall from specific namespace
helm uninstall backend --namespace myapp
```

## Troubleshooting

### Common Issues

1. **Pod not starting**
   ```bash
   kubectl describe pod -l app=backend
   kubectl logs -l app=backend
   ```

2. **Service not accessible**
   ```bash
   kubectl get service backend
   kubectl describe service backend
   ```

3. **Health check failures**
   ```bash
   # Check if health endpoints are responding
   kubectl exec -it deployment/backend -- curl http://localhost:80/health
   kubectl exec -it deployment/backend -- curl http://localhost:80/ready
   ```

4. **Image pull issues**
   ```bash
   # Check image pull policy and registry access
   kubectl describe pod -l app=backend | grep -A 5 "Events:"
   ```

### Debug Commands
```bash
# Check deployment status
kubectl get deployment backend

# Check pod logs
kubectl logs deployment/backend

# Get shell access
kubectl exec -it deployment/backend -- /bin/bash

# Check service endpoints
kubectl get endpoints backend
```

## Security Considerations

- The application runs with default security context
- Consider implementing Pod Security Standards for production
- Use secrets for sensitive configuration
- Implement network policies for traffic isolation
- Regular security scanning of container images

## Development Workflow

1. **Build and tag image**
2. **Push to registry**
3. **Update values with new tag**
4. **Deploy using Helm**
5. **Monitor deployment and health**

## Contributing

When modifying this chart:
1. Update the chart version in `Chart.yaml`
2. Update this README with any new configuration options
3. Test the chart with different value combinations
4. Validate Kubernetes manifests: `helm template . | kubectl apply --dry-run=client -f -`