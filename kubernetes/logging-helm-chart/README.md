# Logging Stack Helm Chart

## Overview
This Helm chart deploys the PLG (Promtail, Loki, Grafana) stack for Kubernetes log collection and visualization.

## Components
- **Promtail**: Collects logs from all pods and forwards them to Loki.
- **Loki**: Aggregates and stores logs.
- **Grafana**: Visualizes logs and metrics.

## Installation
1. Add the Grafana Helm repository:
   ```bash
   helm repo add grafana https://grafana.github.io/helm-charts
   helm repo update
   ```
2. Install the logging stack:
   ```bash
   helm install logging-stack ./kubernetes/logging-helm-chart --create-namespace --namespace logging
   ```

## Configuration
Key configuration options in `values.yaml`:
```yaml
loki:
  persistence:
    enabled: true
    size: 10Gi
promtail:
  extraVolumes:
    - name: positions
      emptyDir: {}
  extraVolumeMounts:
    - name: positions
      mountPath: /data
grafana:
  admin:
    existingSecret: grafana-admin-secret
```