# DevOps Challenge - Infrastructure Architecture

## Overview

This document describes the updated infrastructure architecture and CI/CD pipeline for the DevOps Challenge application. The solution demonstrates a complete containerized microservices deployment with monitoring, logging, and automated infrastructure provisioning.

## Application Architecture

```mermaid
graph TB
    subgraph "External Access"
        U[Users]
    end
    
    subgraph "Kubernetes Cluster (Minikube)"
        
        subgraph "Application Layer"
            SVC[Store API Service<br/>ClusterIP:80]
            POD[Store API Pod<br/>.NET 8 API<br/>Port 80]
        end
        
        subgraph "Monitoring & Logging Stack"
            GR[Grafana<br/>NodePort:30300]
            LK[Loki<br/>ClusterIP:3100]
            PT[Promtail<br/>DaemonSet]
        end
        
    end
    
    U --> SVC
    SVC --> POD
    POD --> LK
    PT --> LK
    GR --> LK
    
    classDef application fill:#e1f5fe
    classDef monitoring fill:#fff3e0
    classDef external fill:#e8f5e8
    
    class POD,SVC application
    class GR,LK,PT monitoring
    class U external
```

## Infrastructure Components

### Core Application
- **Store API**: .NET 8 REST API with CRUD operations
- **Docker Registry**: Local registry for container images
- **Kubernetes Service**: ClusterIP service exposing the API internally

### Monitoring & Logging
- **Grafana**: Visualization and dashboards (accessible via NodePort 30300)
- **Loki**: Log aggregation and storage (community Helm chart)
- **Promtail**: Log collection from all pods (community Helm chart)

### Container Orchestration
- **Minikube**: Local Kubernetes cluster
- **Helm Charts**: Community charts for Promtail, Loki, and Grafana
- **Docker**: Container runtime and image building

## CI/CD Pipeline

```mermaid
graph TD
    subgraph "Source Control"
        GH[GitHub Repository<br/>andonyns/devops-challenge]
    end
    
    subgraph "CI/CD Pipeline (Jenkins)"
        CO[Checkout Code]
        SEC[Security Scan<br/>Checkov]
        TI[Terraform Init]
        TP[Terraform Plan]
        
        CO --> SEC
        SEC --> TI
        TI --> TP
    end
    
    classDef cicd fill:#fff3e0
    
    class GH source
    class CO,SEC,TI,TP,TA cicd
```

## Monitoring & Observability

### Logging Stack
- **Promtail**: Collects logs from all pods
- **Loki**: Aggregates and stores logs
- **Grafana**: Visualizes logs and metrics

### Health Checks
- **Liveness Probes**: `/health` endpoint monitoring
- **Readiness Probes**: `/ready` endpoint monitoring
- **Resource Limits**: CPU and memory constraints

## Continuous Integration Workflow

1. **Code Commit**: Developer pushes to GitHub
2. **Jenkins Trigger**: Pipeline starts manually
3. **Security Scan**: Checkov validates Terraform configurations
4. **Infrastructure Planning**: Terraform plan shows changes

## Deployment Workflow

**Infrastructure Deployment**:
   - Start Minikube cluster
   - Enable required addons (registry, ingress)
   - Build and push Docker images to local registry
   - Deploy application via Helm
   - Deploy monitoring stack

## Access URLs

### Development Environment
- **API**: `http://localhost:{forwarded_port}/items`
- **Grafana**: `http://localhost:30300` (admin/admin)
- **Minikube Dashboard**: `minikube dashboard`

### API Endpoints
- `GET /items` - List all items
- `POST /items` - Create new item
- `GET /items/{id}` - Get specific item
- `PUT /items/{id}` - Update item
- `DELETE /items/{id}` - Delete item
- `GET /health` - Health check
- `GET /ready` - Readiness check

To test the API, you can use the HTTP VSCode Extension changing the port
