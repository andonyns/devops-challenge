# Logging Module

This Terraform module deploys a comprehensive logging infrastructure to a Kubernetes cluster using the PLG (Promtail, Loki, Grafana) stack.

## Components

### Loki
- **Purpose**: Log aggregation system
- **Port**: 3100 (ClusterIP service)
- **Storage**: Temporary filesystem storage (development setup)
- **Configuration**: Accepts logs from Promtail and provides API for Grafana

### Promtail
- **Purpose**: Log collection agent
- **Deployment**: DaemonSet (runs on every node)
- **Function**: Collects logs from Kubernetes pods and ships them to Loki
- **Permissions**: Includes RBAC configuration for cluster-wide log collection

### Grafana
- **Purpose**: Log visualization and dashboarding
- **Port**: 3000 (NodePort service, accessible on port 30300)
- **Default Credentials**: admin/admin
- **Features**: 
  - Pre-configured Loki datasource
  - Kubernetes logs dashboard
  - Log level analysis
  - Real-time log streaming

## Usage

The module is automatically applied when running Terraform and will:
1. Create a `logging` namespace
2. Deploy Loki, Promtail, and Grafana
3. Configure log collection from all Kubernetes pods
4. Set up a Grafana dashboard for log visualization

## Accessing Grafana

After deployment, Grafana will be accessible at:
- **Local (Minikube)**: `http://$(minikube ip):30300`
- **Port Forward**: `kubectl port-forward -n logging svc/logging-stack-grafana 3000:3000`

## Dashboard Features

The included Kubernetes logs dashboard provides:
- Log level distribution (pie chart)
- Log rate over time
- Real-time log stream
- Filtering by namespace, pod, and container

## Dependencies

This module depends on a running Kubernetes cluster (provided by the `cluster` module).

## Configuration

The module uses default configurations suitable for development environments. For production use, consider:
- Enabling persistent storage for Loki and Grafana
- Configuring log retention policies
- Setting up proper resource limits
- Implementing log aggregation rules