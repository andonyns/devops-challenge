# Logging Stack Helm Chart

This Helm chart deploys a complete logging infrastructure using the PLG (Promtail, Loki, Grafana) stack for Kubernetes log collection and visualization.

## Installation

```bash
helm install logging-stack ./kubernetes/logging-helm-chart --create-namespace --namespace logging
```

## Components

### Loki
- **Image**: grafana/loki:2.9.3
- **Service**: ClusterIP on port 3100
- **Storage**: Temporary filesystem (suitable for development)

### Promtail
- **Image**: grafana/promtail:2.9.3
- **Deployment**: DaemonSet for cluster-wide log collection
- **RBAC**: Includes necessary permissions for log collection

### Grafana
- **Image**: grafana/grafana:10.2.2
- **Service**: NodePort on port 30300
- **Default Credentials**: admin/admin
- **Dashboards**: Pre-configured Kubernetes logs dashboard

## Configuration

Key configuration options in `values.yaml`:

```yaml
loki:
  image: grafana/loki:2.9.3
  replicas: 1
  service:
    port: 3100
    type: ClusterIP

promtail:
  image: grafana/promtail:2.9.3

grafana:
  image: grafana/grafana:10.2.2
  service:
    port: 3000
    type: NodePort
    nodePort: 30300
  adminPassword: admin
```

## Accessing Services

- **Grafana**: http://localhost:30300 (after port-forwarding or using Minikube IP)
- **Loki**: Internal service at `logging-stack-loki:3100`

## Log Collection

Promtail is configured to collect logs from:
- All Kubernetes pods with the `name` label
- All Kubernetes pods with the `app` label
- Automatic service discovery and labeling

## Dashboard Features

The included dashboard provides:
- Log level distribution
- Log rate monitoring
- Real-time log streaming
- Pod and namespace filtering

## Customization

To customize the deployment:
1. Modify `values.yaml` for configuration changes
2. Add custom dashboards in `templates/grafana.yaml`
3. Update Promtail configuration in `templates/promtail.yaml`
4. Configure Loki settings in `templates/loki.yaml`

## Production Considerations

For production deployments, consider:
- Enabling persistent storage for Loki and Grafana
- Configuring proper resource limits and requests
- Setting up log retention policies
- Implementing security policies and network restrictions
- Using proper storage classes for persistent volumes