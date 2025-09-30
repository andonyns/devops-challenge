# DevOps Challenge

## Overview
This repository contains the infrastructure and application code for the DevOps Challenge. It demonstrates a complete containerized microservices deployment with monitoring, logging, and automated infrastructure provisioning.

## Features
- **Containerized Application**: .NET 8 REST API with CRUD operations.
- **Infrastructure as Code**: Terraform for provisioning Kubernetes clusters and resources.
- **Monitoring and Logging**: Grafana, Loki, and Promtail for observability.
- **CI/CD Pipeline**: Jenkins pipeline for automated builds, tests, and deployments.
- **Cert-Manager Integration**: Automated SSL/TLS certificate management using Let's Encrypt.

## Prerequisites
- Docker
- Kubernetes (Minikube or any other cluster)
- Helm
- Terraform
- Jenkins

## Setup Instructions

### 1. Clone the Repository
```bash
git clone https://github.com/andonyns/devops-challenge.git
cd devops-challenge
```

### 2. Deploy Infrastructure
1. Initialize Terraform:
   ```bash
   terraform init
   ```
2. Plan and apply the infrastructure:
   ```bash
   terraform plan -var-file=terraform/dev.tfvars
   terraform apply -var-file=terraform/dev.tfvars
   ```

### 3. Deploy Application and Monitoring Stack
1. Install Helm dependencies:
   ```bash
   helm repo add grafana https://grafana.github.io/helm-charts
   helm repo update
   ```
2. Deploy the logging stack:
   ```bash
   helm install logging-stack kubernetes/logging-helm-chart
   ```
3. Deploy the application:
   ```bash
   helm install application kubernetes/application-helm-chart
   ```

### 4. Configure Cert-Manager
1. Install cert-manager:
   ```bash
   helm repo add jetstack https://charts.jetstack.io
   helm repo update
   helm install cert-manager jetstack/cert-manager \
     --namespace cert-manager \
     --create-namespace \
     --set installCRDs=true
   ```
2. Create a ClusterIssuer:
   ```yaml
   apiVersion: cert-manager.io/v1
   kind: ClusterIssuer
   metadata:
     name: letsencrypt-prod
   spec:
     acme:
       server: https://acme-v02.api.letsencrypt.org/directory
       email: your-email@example.com
       privateKeySecretRef:
         name: letsencrypt-prod
       solvers:
         - http01:
             ingress:
               class: nginx
   ```
   Apply the configuration:
   ```bash
   kubectl apply -f cluster-issuer.yaml
   ```

### 5. Access the Application
- **Store API**: `http://store-api.local`
- **Grafana**: `http://localhost:30300` (admin/admin)
- **Minikube Dashboard**: `minikube dashboard`

## Contributing
Feel free to submit issues or pull requests to improve this project.

## License
This project is licensed under the MIT License.